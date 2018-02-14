using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastTeamStandingWinTrackerTests {

    /**
     * Ensures that if there is only one team with units still with more than 0 health, that team will be determined as the winner.
     */ 
	[Test]
    public void DeterminesCorrectWinner()
    {

        CheckWinner(1, new Unit.IInstance[,]
        {
            { new MockUnit("Test Unit 1", 0, 0, 0, true), new MockUnit("Test Unit 2", 1, 0, 0, true), new MockUnit("Test Unit 3", 2, 0, 0, true)},
            { new MockUnit("Test Unit 4", 0, 0, 0, true), new MockUnit("Test Unit 5", 1, 3, 0, true), new MockUnit("Test Unit 6", 2, 0, 0, true) },
            { new MockUnit("Test Unit 7", 0, 0, 0, true), new MockUnit("Test Unit 8", 1, 78, 0, true), new MockUnit("Test Unit 9", 2, 0, 0, true) }
        });

        CheckWinner(2, new Unit.IInstance[,]
        {
            { new MockUnit("Test Unit 1", 0, 0, 0, true), new MockUnit("Test Unit 2", 1, 0, 0, true), new MockUnit("Test Unit 3", 2, 2, 0, true)},
            { new MockUnit("Test Unit 4", 0, 0, 0, true), new MockUnit("Test Unit 5", 1, 0, 0, true), new MockUnit("Test Unit 6", 2, 0, 0, true) },
            { new MockUnit("Test Unit 7", 0, 0, 0, true), new MockUnit("Test Unit 8", 1, -3, 0, true), new MockUnit("Test Unit 9", 2, 0, 0, true) }
        });

        CheckWinner(6, new Unit.IInstance[,]
        {
            { new MockUnit("Test Unit 1", 0, 0, 0, true), new MockUnit("Test Unit 2", 1, 0, 0, true), new MockUnit("Test Unit 3", 2, 0, 0, true)},
            { new MockUnit("Test Unit 4", 0, 0, 0, true), new MockUnit("Test Unit 5", 1, 0, 0, true), new MockUnit("Test Unit 6", 2, 0, 0, true) },
            { new MockUnit("Test Unit 7", 6, 7, 0, true), new MockUnit("Test Unit 8", 1, 0, 0, true), new MockUnit("Test Unit 9", 2, 0, 0, true) }
        });

    }

    /**
     * Assert that the given winner can be determined from the given board.
     */ 
    private void CheckWinner(int expectedWinner, Unit.IInstance[,] board)
    {
        int actualWinner = new LastTeamStandingWinTracker().DetermineWinner(board);
        Assert.AreEqual(expectedWinner, actualWinner, "Team " + expectedWinner + " should have won, but Team " + actualWinner + " did instead.");
    }

    /**
     * Ensures that when there is more than one team with units with more than 0 health, it is determined there is no winner.
     */ 
    [Test]
    public void DeterminesWhenThereIsNoWinnerCorrectly()
    {

        CheckNoWinner(new Unit.IInstance[,]
        {
            { new MockUnit("Test Unit 1", 0, 9, 0, true), new MockUnit("Test Unit 2", 1, 0, 0, true), new MockUnit("Test Unit 3", 2, 0, 0, true)},
            { new MockUnit("Test Unit 4", 0, 0, 0, true), new MockUnit("Test Unit 5", 1, 3, 0, true), new MockUnit("Test Unit 6", 2, 0, 0, true) },
            { new MockUnit("Test Unit 7", 0, 56, 0, true), new MockUnit("Test Unit 8", 1, 78, 0, true), new MockUnit("Test Unit 9", 2, 0, 0, true) }
        });

        CheckNoWinner(new Unit.IInstance[,]
        {
            { new MockUnit("Test Unit 1", 0, 9, 0, true), new MockUnit("Test Unit 2", 1, 0, 0, true), new MockUnit("Test Unit 3", 2, 7, 0, true)},
            { new MockUnit("Test Unit 4", 0, 0, 0, true), new MockUnit("Test Unit 5", 1, 0, 0, true), new MockUnit("Test Unit 6", 2, 0, 0, true) },
            { new MockUnit("Test Unit 7", 0, 56, 0, true), new MockUnit("Test Unit 8", 1, 0, 0, true), new MockUnit("Test Unit 9", 2, -2, 0, true) }
        });
    }

    /**
     * Assert that the given board does not produce a winner.
     */ 
    private void CheckNoWinner(Unit.IInstance[,] board)
    {
        int actualWinner = new LastTeamStandingWinTracker().DetermineWinner(board);
        Assert.AreEqual(-1, actualWinner, "Team " + actualWinner + " is said to have won, but there should be no winner.");
    }

    /**
     * Ensures that an error is thrown if there is no team unit on the board with more than 0 health.
     */ 
    [Test]
    public void ExcpetionThrownWhenNoTeamIsStanding()
    {

        CheckNoTeamStandingException(new Unit.IInstance[,]
        {
            { new MockUnit("Test Unit 1", 0, 0, 0, true), new MockUnit("Test Unit 2", 1, 0, 0, true), new MockUnit("Test Unit 3", 2, 0, 0, true)},
            { new MockUnit("Test Unit 4", 0, 0, 0, true), new MockUnit("Test Unit 5", 1, 0, 0, true), new MockUnit("Test Unit 6", 2, 0, 0, true) },
            { new MockUnit("Test Unit 7", 0, 0, 0, true), new MockUnit("Test Unit 8", 1, 0, 0, true), new MockUnit("Test Unit 9", 2, 0, 0, true) }
        });

        CheckNoTeamStandingException(new Unit.IInstance[,]
        {
            { new MockUnit("Test Unit 1", 0, 0, 0, true), new MockUnit("Test Unit 2", 1, -1, 0, true), new MockUnit("Test Unit 3", 2, 0, 0, true)},
            { new MockUnit("Test Unit 4", 0, 0, 0, true), new MockUnit("Test Unit 5", 1, 0, 0, true), new MockUnit("Test Unit 6", 2, 0, 0, true) },
            { new MockUnit("Test Unit 7", 0, 0, 0, true), new MockUnit("Test Unit 8", 1, 0, 0, true), new MockUnit("Test Unit 9", 2, 0, 0, true) }
        });

    }

    /**
     * Asserts that the given board will cause a NoTeamStandingException to be thrown.
     */ 
    private void CheckNoTeamStandingException(Unit.IInstance[,] board)
    {
        try
        {
            new LastTeamStandingWinTracker().DetermineWinner(board);
            Assert.Fail("An exception should be thrown as no unit is standing.");
        }
        catch (LastTeamStandingWinTracker.NoTeamStandingException e) { }

    }

}
