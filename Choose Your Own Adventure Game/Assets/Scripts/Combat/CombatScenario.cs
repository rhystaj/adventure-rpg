using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CombatScenario : ICombatScenario
{

    private Once<Unit.IInstance[,]> board = new Once<Unit.IInstance[,]>(); //The grid of units.
    private Once<CombatFlow> flow = new Once<CombatFlow>(); //The struture of the progression of the combat.
    private Once<WinTracker> winTracker = new Once<WinTracker>(); //Tracks the progress of the game to determine the winner.

    public delegate void TeamEvent(int team);
    public TeamEvent OnTeamWin;

    /**
     * Return a copy of the board.
     */
    public Unit.IInstance[,] Board {

        get {

            Unit.IInstance[,] clone = new Unit.IInstance[board.Value.GetLength(0), board.Value.GetLength(1)];
            System.Array.Copy(board.Value, clone, clone.Length);

            return clone;

        }

    }

    public HashSet<Unit.IInstance> AvaliableUnits { get { return flow.Value.UnitsAvaliableForTurn; } }

    public CombatScenario(Unit.IInstance[,] playerUnits, ICombatEncounter encounter, CombatFlow.Adaptor flowAdaptor, WinTracker winTracker)
    {

        //Preconditions
        Assert.IsTrue(playerUnits.GetLength(0) == encounter.rows && playerUnits.GetLength(1) == encounter.columnsPerSide,
                      "Precondition Fail: The player configuration should have the same dimensions and the enemy configuration.");
        Assert.IsNotNull(encounter, "Precondition Fail: The argument 'encounter' should not be null.");
        Assert.IsNotNull(flowAdaptor, "Precondition Fail: The argument 'flowAdaptor' should not be null.");
        Assert.IsNotNull(winTracker, "Precondition Fail: The argument 'winTracker' should not be null.");

        //Create a new board
        board.Value = new Unit.IInstance[encounter.rows, encounter.columnsPerSide * 2]; //Double as columns per side is the number of units on ONE of the TWO sides.

        
        //Copy the player units to the player's side and set thier positions based on how far they are away from the centre.
        for(int i = 0; i < board.Value.GetLength(0); i++)
        {
            for(int j = 0; j < encounter.columnsPerSide; j++)
            {
                board.Value[i, j] = playerUnits[i, j];
                board.Value[i, j].position = Mathf.Abs(j - (encounter.columnsPerSide - 1));
            }
        }


        //Copy the enemy units from the encounter to the enemy side and set thier positions based on how far away they are from the centre.
        for (int i = 0; i < board.Value.GetLength(0); i++)
        {
            for (int j = encounter.columnsPerSide; j < board.Value.GetLength(1); j++)
            {
                board.Value[i, j] = encounter.instantiatedEnemyConfiguration[(j - encounter.columnsPerSide) + (encounter.columnsPerSide * i)];
                board.Value[i, j].position = Mathf.Abs(j - (encounter.columnsPerSide));
            }
        }

        flow.Value = flowAdaptor.Convert(playerUnits, encounter);


        this.winTracker.Value = winTracker;


        //Postconditions
        Assert.IsNotNull(board, "Postcondition Fail: The field 'board' should not be null.");
        Assert.IsTrue(board.Value.Length == encounter.columnsPerSide * encounter.rows * 2);
        Assert.IsNotNull(flow, "Postcondition Fail: The field 'flowAdaptor' should not be null.");
        Assert.IsNotNull(winTracker, "Postcondition Fail: The field 'winTracker' should not be null.");

    }

    /**
     * Try to register a turn in which a unit uses their instrument on another.
     */
    public bool UseInstrument(Unit.IInstance subject, Unit.IInstance target)
    {

        //Preconditions
        Assert.IsNotNull(subject, "Precondition Fail: The argument 'subject' should not be null.");
        Assert.IsTrue(subject.health > 0, "Precondition Fail: 'subject' should have more than 0 health.");
        Assert.IsNotNull(target, "Precondition Fail: The argument 'target' should not be null.");
        Assert.IsNotNull(OnTeamWin, "The delegate 'OnTeamWin' should not be null.");


        bool result = false;

        if (flow.Value.CanMoveDuringCurrentTurn(subject) && subject.UseInstrument(target))
        {
            flow.Value.TakeTurn(subject);
            winTracker.Value.Update(board.Value);
            result = true;
        }

        if (result)
        {
            int winner = winTracker.Value.DetermineWinner(board.Value);
            if (winner != -1) OnTeamWin(winner);
        }


        return result;
    }


    public interface WinTracker
    {

        /**
         * Updates the status of the tracker that will go towards determining the winner.
         */ 
        void Update(Unit.IInstance[,] board);

        /**
         * Returns the winning team, or -1 if there is none.
         */ 
        int DetermineWinner(Unit.IInstance[,] board);

    }

}