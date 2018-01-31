using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NSubstitute;
using System.Collections.Generic;

public class CombatFlowTests {

    public const string FLOW_TEST_UNITS_PATH = "Testing/Mock Prefabs/Combat/Units/Flow Test Units";

    /**
     * Ensures that a unit can move in a turn in which it is listed in avaliable units.
     */
    [Test]
    public void AUnitInAvaliableUnitsCanMove()
    {

        Unit targetUnit = GetFlowTestUnit("Flow Test Mock Unit 1");
        HashSet<IUnit> avalaibleUnits = new HashSet<IUnit>(new IUnit[] {
            targetUnit,
            GetFlowTestUnit("Flow Test Mock Unit 2"),
            GetFlowTestUnit("Flow Test Mock Unit 3")
        });
        CombatFlow testFlow = new SingleEmptyTurnMockCombatFlow(1, avalaibleUnits);

        Assert.IsTrue(testFlow.CanMoveDuringCurrentTurn(targetUnit));

    }

    /**
     * Ensures that a unit can't move in a turn in which it is not listed in avaliable units.
     */
    [Test]
    public void AUnitNotInAvaliableUnitsCantMove()
    {

        Unit targetUnit = GetFlowTestUnit("Flow Test Mock Unit 1");
        HashSet<IUnit> avalaibleUnits = new HashSet<IUnit>(new IUnit[] {
            GetFlowTestUnit("Flow Test Mock Unit 2"),
            GetFlowTestUnit("Flow Test Mock Unit 3")
        });
        CombatFlow testFlow = new SingleEmptyTurnMockCombatFlow(1, avalaibleUnits);

        Assert.IsFalse(testFlow.CanMoveDuringCurrentTurn(targetUnit));

    }

    [Test]
    public void ANewTurnIsNotReturnedWhenAUnitThatCantMoveIsGiven()
    {

        Unit targetUnit = GetFlowTestUnit("Flow Test Mock Unit 1");
        HashSet<IUnit> avalaibleUnits = new HashSet<IUnit>(new IUnit[] {
            GetFlowTestUnit("Flow Test Mock Unit 2"),
            GetFlowTestUnit("Flow Test Mock Unit 3")
        });

        CombatFlow testFlow = new SingleEmptyTurnMockCombatFlow(1, avalaibleUnits);


        HashSet<IUnit> prevAvaliableUnits = testFlow.UnitsAvaliableForTurn;
        int prevTeamNumber = testFlow.CurrentTeam;

        testFlow.TakeTurn(targetUnit);


        Assert.IsTrue(prevAvaliableUnits.SetEquals(testFlow.UnitsAvaliableForTurn),
                      "The avaliable units should not change when a unit that can't move is given.");
        Assert.IsTrue(prevTeamNumber == testFlow.CurrentTeam,
                      "The turn number should not change when a unit that can't move is given.");

    }

    public static Unit GetFlowTestUnit(string name)
    {

        GameObject prefab = Resources.Load<GameObject>(FLOW_TEST_UNITS_PATH + "/" + name);
        return GameObject.Instantiate(prefab).GetComponent<Unit>();

    }

}
