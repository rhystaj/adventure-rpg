using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A mock combat flow with a single turn that does nothing when taken.
 */ 
public class SingleEmptyTurnMockCombatFlow : CombatFlow {

	public SingleEmptyTurnMockCombatFlow(int team, HashSet<Unit.IInstance> avaliableUnits)
    {
        currentTurn = new EmptyTurn(team, avaliableUnits);
    }

    private class EmptyTurn : Turn
    {

        public EmptyTurn(int team, HashSet<Unit.IInstance> avaliableUnits) : base(team, avaliableUnits){ /* No addition functionality needed */ }

        protected override Turn ProduceNextTurn(Unit.IInstance subject) { return this; /* Is a mock, so does nothing. */ }

    }


}
