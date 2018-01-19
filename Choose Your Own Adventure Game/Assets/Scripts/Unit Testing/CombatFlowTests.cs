using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NSubstitute;
using System.Collections.Generic;

public class CombatFlowTests {

    /**
     * Ensures that a unit can move in a turn in which it is listed in avaliable units.
     */ 
	[Test]
    public void AUnitInAvaliableUnitsCanMove()
    {

        Unit targetUnit = new Unit(Substitute.For<Instrument>(), 23, 4, 1);
        HashSet<Unit> avalaibleUnits = new HashSet<Unit>(new Unit[] {
            targetUnit,
            new Unit(Substitute.For<Instrument>(), 6, 7, 8),
            new Unit(Substitute.For<Instrument>(), 23, 5, 1)
        });
        CombatFlow testFlow = new SingleEmptyTurnMockCombatFlow(1, avalaibleUnits);

        Assert.IsTrue(testFlow.CanMoveDuringCurrentTurn(targetUnit));

    }

}
