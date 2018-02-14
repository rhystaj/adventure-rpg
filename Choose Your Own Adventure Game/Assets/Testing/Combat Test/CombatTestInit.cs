using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestInit : MonoBehaviour {

    public CombatBoard board;
    public CombatEncounter encounter;
    public Unit playerUnit;

    private void Start()
    {

        Unit.Instance[,] playerUnits = new Unit.Instance[,]
        {
            { new Unit.Instance(playerUnit), new Unit.Instance(playerUnit) },
            { new Unit.Instance(playerUnit), new Unit.Instance(playerUnit) }
        };

        CombatScenario scenario = new CombatScenario(playerUnits, encounter,
                                                     new CombatFlow.DirectAdaptor(new SingleEmptyTurnMockCombatFlow(0, null)),
                                                     new MockWinTracker(0, 5));

        board.Display(scenario.Board);

    }

}
