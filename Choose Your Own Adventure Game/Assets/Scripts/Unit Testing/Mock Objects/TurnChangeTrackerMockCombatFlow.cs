using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Counts the times a turn is taken.
 */ 
public class TurnChangeTrackerMockCombatFlow : CombatFlow {

    public TurnChangeTrackerMockCombatFlow(int team, IUnit[] avaliableUnits)
    {
        //currentTurn = new TrackedTurn(team, new HashSet<IUnit>(new List<IUnit>(avaliableUnits)), )
    }

    private class TrackedTurn : Turn
    {

        public int turnsTaken;

        public TrackedTurn(int team, HashSet<IUnit> avaliableUnits, int turnsTaken) : base(team, avaliableUnits)
        {
        }

        protected override Turn ProduceNextTurn(IUnit turnTaker)
        {
            throw new NotImplementedException();
        }
    }

}
