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

        scenario = new CombatScenario(playerUnits, encounter,
                                     new AllUnitsAvaliableFlow.AllAdaptor(),
                                     new MockWinTracker(0, 5));
        scenario.SetOnWinAction(i => { });

        board.Display(scenario.Board);
        animator = new CombatAnimator(board.vessels, overlay);

        StartCoroutine(Run(new IController[] { board, new RandomAI() }));

    }

    private IEnumerator Run(IController[] controllers)
    {

        while (true) {

            Debug.Log("Team Moving: " + scenario.teamMoving);

            //Wait unil an action has been decided on unit continuing. AIs will never have to be waited on, this is to wait for user input. 
            ICombatAction action = null;
            while (action == null)
            {
                yield return new WaitForEndOfFrame();
                action = controllers[scenario.teamMoving].DetermineMove(scenario.Board, scenario.unitsAvaliableToMove);
            }

            Debug.Log("Animating");

            //Animate the action and wait unit it is done.
            yield return action.Animate(animator);
            action.Perform(scenario);

            Debug.Log("Done Animating");

        }

    }

}
