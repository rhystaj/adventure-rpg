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

        Unit.IInstance targetUnit = new MockUnit("Unit 1", 0, 50, 1, true);
        HashSet<Unit.IInstance> avalaibleUnits = new HashSet<Unit.IInstance>(new Unit.IInstance[] {
            targetUnit,
            new MockUnit("Unit 2", 0, 56, 1, true),
            new MockUnit("Unit 3", 0, 70, 1, true)
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

        Unit.IInstance targetUnit = new MockUnit("Unit 1", 0, 50, 1, true);
        HashSet<Unit.IInstance> avalaibleUnits = new HashSet<Unit.IInstance>(new Unit.IInstance[] {
            new MockUnit("Unit 2", 0, 56, 1, true),
            new MockUnit("Unit 3", 0, 70, 1, true)
        });
        CombatFlow testFlow = new SingleEmptyTurnMockCombatFlow(1, avalaibleUnits);

        Assert.IsFalse(testFlow.CanMoveDuringCurrentTurn(targetUnit));

    }

    [Test]
    public void ANewTurnIsNotReturnedWhenAUnitThatCantMoveIsGiven()
    {

        Unit.IInstance targetUnit = new MockUnit("Unit 1", 0, 50, 1, true);
        HashSet<Unit.IInstance> avalaibleUnits = new HashSet<Unit.IInstance>(new Unit.IInstance[] {
            new MockUnit("Unit 2", 0, 56, 1, true),
            new MockUnit("Unit 3", 0, 70, 1, true)
        });

        CombatFlow testFlow = new SingleEmptyTurnMockCombatFlow(1, avalaibleUnits);


        HashSet<Unit.IInstance> prevAvaliableUnits = testFlow.UnitsAvaliableForTurn;
        int prevTeamNumber = testFlow.CurrentTeam;

        testFlow.TakeTurn(targetUnit);


        Assert.IsTrue(prevAvaliableUnits.SetEquals(testFlow.UnitsAvaliableForTurn),
                      "The avaliable units should not change when a unit that can't move is given.");
        Assert.IsTrue(prevTeamNumber == testFlow.CurrentTeam,
                      "The turn number should not change when a unit that can't move is given.");

    }

}
