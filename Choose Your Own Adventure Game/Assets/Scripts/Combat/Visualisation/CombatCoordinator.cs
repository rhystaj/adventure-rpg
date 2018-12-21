using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * Oversees the combat.
 */
public class CombatCoordinator : MonoBehaviour {

    [SerializeField] CombatBoard board; //The visual representation of combat being manipulated to display the combat.
    public Unit playerUnit;
    public CombatEncounter encounter;

    private ICombatScenario scenario; //The scenario being visualised.
    private CombatAnimator animator; //The animator used to manipulate the pieces on the board.

    private void Start()
    {

        Unit.Instance[,] playerUnits = new Unit.Instance[,]
        {
            { new Unit.Instance(playerUnit), new Unit.Instance(playerUnit) },
            { new Unit.Instance(playerUnit), new Unit.Instance(playerUnit) }
        };


        //Create a CombatScenario, which determines the underlying structure of the match, from whan units can move to what counts as a win.
        scenario = new CombatScenario(playerUnits, encounter,
                                     new PredeterminedOrderFlow.PDOAdaptor(
                                         new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) }, 
                                         new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) }),
                                     new MockWinTracker(0, 5));
        scenario.SetOnWinAction(i => { });


        //Set up the board, based on the configuration of units in the sceanrio, and add interaction listeners.
        board.Display(scenario.Board);

        animator = new CombatAnimator(board.vessels);

        StartCoroutine(Run(new IController[] { new UserController(board), new RandomAI() }));

    }

    private IEnumerator Run(IController[] controllers)
    {

        while (true) {

            IController currentController = controllers[scenario.teamMoving];
            currentController.PrepareForMove(scenario.Board, scenario.unitsAvaliableToMove);


            //Wait unil an action has been decided on unit continuing. AIs will never have to be waited on, this is to wait for user input. 
            ICombatAction action = null;
            while (action == null)
            {
                yield return new WaitForEndOfFrame();
                action = currentController.DetermineMove(scenario.Board, scenario.unitsAvaliableToMove);
            }
            currentController.PostMove(scenario.Board, scenario.unitsAvaliableToMove);

            //Animate the action and wait unit it is done.
            yield return action.Perform(animator, scenario);


        }

    }

    /**
     * A controller that determines the move based on user input via the board.
     */ 
    public class UserController : IController
    {

        private Once<CombatBoard> gameboard = new Once<CombatBoard>(); //The board used to register user input.

        private Unit.IInstance subject;
        private Unit.IInstance target;

        public UserController(CombatBoard board)
        {

            //Preconditions
            Assert.IsNotNull(board, "Precondition Fail: The argument 'board' should not be null.");


            gameboard.Value = board;
            gameboard.Value.UnitVesselSelected += VesselSelected;


            //Postconditions
            Assert.IsNotNull(gameboard.Value, "Postcondition Fail: The field 'board' should not be null.");


        }

        public void VesselSelected(CombatBoard.UnitVessel vessel)
        {

            //Precondtions
            Assert.IsNotNull(vessel, "Precondition Fail: The argument 'vessel' should not be null");

            if (subject == null)
            {
                subject = vessel.unit;
                gameboard.Value.SpecifyValidUnits(u => subject.CanUseInstrumentOn(u));
            }
            else if (target == null) target = vessel.unit;

        }

        public void PrepareForMove(Unit.IInstance[,] board, HashSet<Unit.IInstance> avaliableUnits)
        {

            //Preconditions
            Assert.IsNotNull(avaliableUnits, "The argument 'avaliableUnits should not be null.'");


            //Unlock the board so that it can be used by the player.
            gameboard.Value.locked = false;


            //Only highlight the units that are avaliable to be moved.
            gameboard.Value.SpecifyValidUnits(u => avaliableUnits.Contains(u));

        }

        public ICombatAction DetermineMove(Unit.IInstance[,] board, HashSet<Unit.IInstance> avaliableUnits)
        {

            if (subject != null & target != null)
            {
                return new InstrumentUse(subject, target);
            }
            else return null;

        }

        public void PostMove(Unit.IInstance[,] board, HashSet<Unit.IInstance> avaliableUnits) {

            //Reset values tracking values for next time.
            subject = null;
            target = null;


            gameboard.Value.locked = true;

        }

    }

}
