
using System.Collections.Generic;


/**
 * Specifies the turn progression of a combat scenario.
 */ 
public abstract class CombatFlow {

    protected Turn currentTurn; //The next turn to be taken.

    public int CurrentTeam { get { return currentTurn.Team; } } //Returns the team taking the current turn.

    //Returns the units avaliable for the current turn.
    public HashSet<Unit> UnitsAvaliableForTurn { get { return new HashSet<Unit>(currentTurn.AvaliableUnits); } }

    public void TakeTurn(Unit subject)
    {
        currentTurn = currentTurn.take(subject);
    }

    /**
     * Determines whether the given unit can move during the current turn.
     */ 
    public bool CanMoveDuringCurrentTurn(Unit unit)
    {
        return currentTurn.CanMove(unit);
    }

    /**
     * Represents a single turn.
     */ 
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

        /**
         * Determins whether the given unit can move during the turn.
         */ 
        public bool CanMove(Unit unit)
        {
            return avaliableUnits.Contains(unit);
        }

    }

}
