using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Specifies the turn progression of a combat scenario.
 */ 
public abstract class CombatFlow {

    protected Turn currentTurn; //The next turn to be taken.

    public int CurrentTeam { get { return currentTurn.Team; } } //Returns the team taking the current turn.

    //Returns the units avaliable for the current turn.
    protected HashSet<Unit> UnitsAvaliableForTurn { get { return new HashSet<Unit>(currentTurn.AvaliableUnits); } }

    public void TakeTurn()
    {
        currentTurn = currentTurn.take();
    }

    protected abstract class Turn
    {
        private int team;
        private HashSet<Unit> avaliableUnits;

        public int Team { get { return team; } }
        public HashSet<Unit> AvaliableUnits { get { return avaliableUnits; } }

        public Turn(int team, HashSet<Unit> avaliableUnits)
        {
            this.team = team;
            this.avaliableUnits = avaliableUnits;
        }

        /**
         * Have the unit take the turn, if they are allowed to.
         */ 
        public abstract Turn take(Unit subject);

        public bool CanMove(Unit unit)
        {
            return avaliableUnits 
        }

    }

}
