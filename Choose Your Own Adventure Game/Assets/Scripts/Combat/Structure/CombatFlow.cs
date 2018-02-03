
using System;
using System.Collections.Generic;


/**
 * Specifies the turn progression of a combat scenario.
 */ 
public abstract class CombatFlow {

    protected Turn currentTurn; //The next turn to be taken.

    public int CurrentTeam { get { return currentTurn.Team; } } //Returns the team taking the current turn.

    //Returns the units avaliable for the current turn.
    public HashSet<Unit.IInstance> UnitsAvaliableForTurn { get { return new HashSet<Unit.IInstance>(currentTurn.AvaliableUnits); } }

    public virtual void TakeTurn(Unit.IInstance subject)
    {
        currentTurn = currentTurn.take(subject);
    }

    /**
     * Determines whether the given unit can move during the current turn.
     */ 
    public bool CanMoveDuringCurrentTurn(Unit.IInstance unit)
    {
        return currentTurn.CanMove(unit);
    }

    /**
     * Represents a single turn.
     */ 
    protected abstract class Turn
    {
        private int team;
        private HashSet<Unit.IInstance> avaliableUnits;

        public int Team { get { return team; } }
        public HashSet<Unit.IInstance> AvaliableUnits { get { return avaliableUnits; } }

        public Turn(int team, HashSet<Unit.IInstance> avaliableUnits)
        {
            this.team = team;
            this.avaliableUnits = avaliableUnits;
        }

        /**
         * Have the unit take the turn, if they are allowed to.
         */ 
        public Turn take(Unit.IInstance subject)
        {

            if (CanMove(subject)) return ProduceNextTurn(subject);
            else return this;

        }

        protected abstract Turn ProduceNextTurn(Unit.IInstance turnTaker);

        /**
         * Determins whether the given unit can move during the turn.
         */ 
        public bool CanMove(Unit.IInstance unit)
        {
            return avaliableUnits.Contains(unit);
        }

    }

    /**
     * A tuple for a selected team and unit.
     */ 
    protected class Selection
    {

        private int selectedTeam;
        public int team { get { return selectedTeam; } }

        private Unit.IInstance selectedUnit;
        public Unit.IInstance unit { get { return selectedUnit; } }

        public Selection(int team, Unit.IInstance unit)
        {
            selectedTeam = team;
            selectedUnit = unit;
        }

    }

    /**
     * An exception to be thrown when there are no valid turns left in the flow.
     */ 
    public class NoValidNextTurnException : Exception { }

    /**
     * Creates a combat flow from the given, more specific than added to the constructor itself, information.
     */ 
    public interface Adaptor { CombatFlow Convert(Unit.IInstance[,] playerTeam, ICombatEncounter encounter); }

    /**
     * An adaptor that will simply produce the CombatFlow given to it. Used mainly for testing.
     */ 
    public class DirectAdaptor : Adaptor
    {

        CombatFlow flow;

        public DirectAdaptor(CombatFlow flow)
        {
            this.flow = flow;
        }

        public CombatFlow Convert(Unit.IInstance[,] playerTeam, ICombatEncounter encounter)
        {
            return flow;
        }

    }

}

