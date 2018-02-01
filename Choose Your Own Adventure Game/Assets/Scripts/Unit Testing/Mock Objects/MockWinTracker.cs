using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A win tracker for testing that simply returns a given winner after a given amount of updates.
 */ 
public class MockWinTracker : CombatScenario.WinTracker
{

    private int winner;
    private int afterTurns;

    private int turns = 0;
    public int turnsTaken { get { return turns; } }

    public MockWinTracker(int winner, int afterTurns)
    {
        this.winner = winner;
        this.afterTurns = afterTurns;
    }

    public int DetermineWinner(IUnit[,] board)
    {
        if (turns < afterTurns) return -1;
        else return winner;
    }

    public void Update(IUnit[,] board)
    {
        turns++;
    }
}
