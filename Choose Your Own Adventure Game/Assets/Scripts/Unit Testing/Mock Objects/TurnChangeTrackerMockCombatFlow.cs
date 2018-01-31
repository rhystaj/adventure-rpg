using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A mock combat flow that counts everytime a turn is sucessfully taken.
 */ 
public class TurnChangeTrackerMockCombatFlow : CombatFlow {

    public int TurnsTaken { get { return ((TrackedTurn)currentTurn).turnsTaken; } }

    public TurnChangeTrackerMockCombatFlow(int team, IUnit[] avaliableUnits)
    {
        currentTurn = new TrackedTurn(team, new HashSet<IUnit>(new List<IUnit>(avaliableUnits)), 0);
    }

    private class TrackedTurn : Turn
    {

        public int turnsTaken;

        public TrackedTurn(int team, HashSet<IUnit> avaliableUnits, int turnsTaken) : base(team, avaliableUnits)
        {
            this.turnsTaken = turnsTaken;
        }

        protected override Turn ProduceNextTurn(IUnit turnTaker)
        {
            return new TrackedTurn(Team, AvaliableUnits, turnsTaken++);
        }
    }

}
