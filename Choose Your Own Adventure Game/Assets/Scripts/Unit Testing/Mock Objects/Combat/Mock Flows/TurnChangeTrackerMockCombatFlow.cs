using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A mock combat flow that counts everytime a turn is sucessfully taken.
 */ 
public class TurnChangeTrackerMockCombatFlow : CombatFlow {

    public int TurnsTaken { get { return ((TrackedTurn)currentTurn).turnsTaken; } }

    public TurnChangeTrackerMockCombatFlow(int team, Unit.IInstance[] avaliableUnits)
    {
        currentTurn = new TrackedTurn(team, new HashSet<Unit.IInstance>(new List<Unit.IInstance>(avaliableUnits)), 0);
    }

    private class TrackedTurn : Turn
    {

        public int turnsTaken;

        public TrackedTurn(int team, HashSet<Unit.IInstance> avaliableUnits, int turnsTaken) : base(team, avaliableUnits)
        {
            this.turnsTaken = turnsTaken;
        }

        protected override Turn ProduceNextTurn(Unit.IInstance turnTaker)
        {
            return new TrackedTurn(Team, AvaliableUnits, turnsTaken++);
        }
    }

}
