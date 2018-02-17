using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Oversees the combat.
 */ 
public class CombatCoordinator : MonoBehaviour {

    [SerializeField] CombatBoard board; //The visual representation of combat being manipulated to display the combat.
    [SerializeField] CombatHUD overlay; //The GUI overlay that display the various stats.
    public Unit playerUnit;
    public CombatEncounter encounter;

    private ICombatScenario scenario; //The scenario being visualised.
    private ICombatAnimator animator; //The animator used to manipulate the pieces on the board.

    private void Start()
    {

        Unit.Instance[,] playerUnits = new Unit.Instance[,]
        {
            { new Unit.Instance(playerUnit), new Unit.Instance(playerUnit) },
            { new Unit.Instance(playerUnit), new Unit.Instance(playerUnit) }
        };


        //Create a CombatScenario, which determines the underlying structure of the match, from whan units can move to what counts as a win.
        scenario = new CombatScenario(playerUnits, encounter,
                                     new AllUnitsAvaliableFlow.AllAdaptor(),
                                     new MockWinTracker(0, 5));
        scenario.SetOnWinAction(i => { });


        //Set up the board, based on the configuration of units in the sceanrio, and add interaction listeners.
        board.Display(scenario.Board);
        board.VesselHighlightBegins = vessel => overlay.HighlightUnit(vessel);
        board.VesselHighlightEnds = () => overlay.HideHighlight();
        board.VesselSelected = vessel => overlay.SelectUnit(vessel);


        animator = new CombatAnimator(board.vessels, overlay);

        StartCoroutine(Run(new IController[] { board, new RandomAI() }));

    }

    private IEnumerator Run(IController[] controllers)
    {

        while (true) {

            //Wait unil an action has been decided on unit continuing. AIs will never have to be waited on, this is to wait for user input. 
            ICombatAction action = null;
            while (action == null)
            {
                yield return new WaitForEndOfFrame();
                action = controllers[scenario.teamMoving].DetermineMove(scenario.Board, scenario.unitsAvaliableToMove);
            }


            //Animate the action and wait unit it is done.
            yield return action.Perform(animator, scenario);


            overlay.HideSelection();

        }

    }

}
