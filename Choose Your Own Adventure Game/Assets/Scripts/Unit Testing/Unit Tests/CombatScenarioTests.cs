using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScenarioTests {

    //General test fields.
    private Unit.IInstance[,] playerTeam;
    private MockCombatEncounter encounter;

    //Fields for testing delegate.
    private int winningTeam;
    private int afterTurns;
    private MockWinTracker mockWinTracker;

    [OneTimeSetUp][TearDown]
    public void GenerateTeams()
    {

        //Create test player team.
        playerTeam = new Unit.IInstance[,]
        {
            { new MockUnit("Player Unit 1", 0, 50, 4, true), new MockUnit("Player Unit 2", 0, 76, 7, true)},
            { new MockUnit("Player Unit 3", 0, 89, 3, false), new MockUnit("Player Unit 4", 0, 4, 2, true)}
        };


        //Create test enemy encounter.
        Unit.IInstance[] enemyTeam = new Unit.IInstance[]
        {
             new MockUnit("Enemy Unit 1", 1, 50, 4, true), new MockUnit("Enemy Unit 2", 1, 76, 7, true),
             new MockUnit("Enemy Unit 3", 1, 89, 3, true), new MockUnit("Enemy Unit 4", 1, 4, 2, true)
        };

        encounter = new MockCombatEncounter(2, 2, new List<Unit.IInstance>(enemyTeam));

    }


    /**
     * Ensures the copy of the board returned by the Board getter is correct.
     */ 
    [Test]
    public void CanGetCorrectCloneOfBoard()
    {

        Unit.IInstance[,] expectedArray = new Unit.IInstance[,]
        {
            { playerTeam[0,0], playerTeam[0,1], encounter.instantiatedEnemyConfiguration[0], encounter.instantiatedEnemyConfiguration[1] },
            { playerTeam[1,0], playerTeam[1,1], encounter.instantiatedEnemyConfiguration[2], encounter.instantiatedEnemyConfiguration[3] }
        };


        //Create the combat scenario and get its board.
        CombatScenario combatScenario = new CombatScenario(playerTeam, encounter,
                                                           new CombatFlow.DirectAdaptor(new TurnChangeTrackerMockCombatFlow(0, new Unit.IInstance[] { })),
                                                           new MockWinTracker(0, 5));
        combatScenario.OnTeamWin = OnTeamWinTestDelegate;
        Unit.IInstance[,] returned = combatScenario.Board;


        //Compare the two arrays.
        for (int i = 0; i < returned.GetLength(0); i++)
            for (int j = 0; j < returned.GetLength(1); j++)
                Assert.AreEqual(expectedArray[i,j], returned[i,j],
                              "The unit " + expectedArray[i,j] + " should be at position " + i + ", " + j + ", but " + returned[i, j] + " is instead.");

    }
	
    /**
     * Ensures the position values given to the units by the combat encounter are correct.
     */ 
    [Test]
    public void UnitsGivenCorrectPositions()
    {

        //The correct position values based on the unit positions.
        int[,] correctPositions = new int[,]
        {
            { 1, 0, 0, 1 },
            { 1, 0, 0, 1 }
        };


        //Create the combat scenario and get its board.
        CombatScenario combatScenario = new CombatScenario(playerTeam, encounter,
                                                           new CombatFlow.DirectAdaptor(new TurnChangeTrackerMockCombatFlow(0, new Unit.IInstance[] { })),
                                                           new MockWinTracker(0, 5));
        combatScenario.OnTeamWin = OnTeamWinTestDelegate;
        Unit.IInstance[,] returned = combatScenario.Board;


        //Compare the positions of the units on the board with thier expected positions.
        //Compare the two arrays.
        for (int i = 0; i < returned.GetLength(0); i++)
            for (int j = 0; j < returned.GetLength(1); j++)
                Assert.AreEqual(correctPositions[i,j], returned[i, j].position,
                       "The unit " + returned[i, j] + " should have the position value " + correctPositions[i, j] + " " +
                       "instead it has the value " + returned[i, j].position + ".");

    }

    /**
     * Ensures the OnTeamWin delegate is fired at the right time with the right team.
     */ 
    [Test]
    public void OnTeamDelegateFiredCorrectly()
    {

        winningTeam = 3;
        afterTurns = 3;
        mockWinTracker = new MockWinTracker(winningTeam, afterTurns);

        CombatScenario combatScenario = new CombatScenario(playerTeam, encounter,
                                                           new CombatFlow.DirectAdaptor(
                                                               new TurnChangeTrackerMockCombatFlow(0, new Unit.IInstance[] { playerTeam[0,0] })
                                                           ),
                                                           mockWinTracker);
        combatScenario.OnTeamWin = OnTeamWinTestDelegate;

        for (int i = 0; i < afterTurns; i++)
            combatScenario.UseInstrument(playerTeam[0,0], encounter.instantiatedEnemyConfiguration[0]);

    }

    private void OnTeamWinTestDelegate(int team)
    {
        Assert.IsTrue(mockWinTracker.turnsTaken == afterTurns,
            "The delegate should be fired after " + afterTurns + " turns, instead it was fired after " + (mockWinTracker.turnsTaken) + " turns.");
        Assert.IsTrue(team == winningTeam, "The winning team should be " + winningTeam + ", but it is " + team + " instead.");
    }

    /**
     * Ensures a use of an intrument will not be considered sucessfull if the subject if not a unit avaliable to be moved.
     */ 
    [Test]
    public void InstrumentUseNotSuccessfulIfGivenUserCantMove()
    {
        CombatScenario combatScenario = new CombatScenario(playerTeam, encounter,
                                                           new CombatFlow.DirectAdaptor(
                                                               new TurnChangeTrackerMockCombatFlow(0, new Unit.IInstance[] { playerTeam[0, 0] })
                                                           ),
                                                           new MockWinTracker(0, 5));
        combatScenario.OnTeamWin = EmptyOnTeamWinDelegate;

        Assert.IsFalse(combatScenario.UseInstrument(playerTeam[0, 1], encounter.instantiatedEnemyConfiguration[0]),
                       "The turn should not be valid, as " + playerTeam[0,1] + " should not be avaliable.");

    }

    /**
     * Ensures the use of al instruement will not be considered successful, if it is considered in any way invalid.
     */ 
    [Test]
    public void InstrumentUseNotSuccessfulIfInvalid()
    {

        CombatScenario combatScenario = new CombatScenario(playerTeam, encounter,
                                                           new CombatFlow.DirectAdaptor(
                                                               new TurnChangeTrackerMockCombatFlow(0, new Unit.IInstance[] { playerTeam[1, 0] })
                                                           ),
                                                           new MockWinTracker(0, 5));
        combatScenario.OnTeamWin = EmptyOnTeamWinDelegate;

        Assert.IsFalse(combatScenario.UseInstrument(playerTeam[1,0], encounter.instantiatedEnemyConfiguration[0]),
                       "The turns should not be valid, as " + playerTeam[1,0] + "'s use of their instrument is not valid.");


    }

    /**
     * Enures the WinTracker will not be updated if the given turn is invalid.
     */ 
    [Test]
    public void WinTrackerWillNotUpdateOnInvalidTurn()
    {

        mockWinTracker = new MockWinTracker(3, 3);

        CombatScenario combatScenario = new CombatScenario(playerTeam, encounter,
                                                           new CombatFlow.DirectAdaptor(
                                                               new TurnChangeTrackerMockCombatFlow(0, new Unit.IInstance[] { playerTeam[1, 0] })
                                                           ),
                                                           mockWinTracker);
        combatScenario.OnTeamWin = EmptyOnTeamWinDelegate;

        Assert.IsFalse(combatScenario.UseInstrument(playerTeam[1, 0], encounter.instantiatedEnemyConfiguration[0]),
                       "The turns should not be valid, as " + playerTeam[1, 0] + "'s use of their instrument is not valid.");
        Assert.IsTrue(mockWinTracker.turnsTaken == 0, "The tracker should not have been uodated as the given move was invalid.");

    }

    /**
     * Ensures the flow will not be progressed if the given turn is invalid.
     */ 
    [Test]
    public void FlowNotProgressedOnInvalidTurn()
    {

        TurnChangeTrackerMockCombatFlow testFlow = new TurnChangeTrackerMockCombatFlow(0, new Unit.IInstance[] { playerTeam[1, 0] });

        CombatScenario combatScenario = new CombatScenario(playerTeam, encounter, new CombatFlow.DirectAdaptor(testFlow), new MockWinTracker(0, 5));
        combatScenario.OnTeamWin = EmptyOnTeamWinDelegate;

        Assert.IsFalse(combatScenario.UseInstrument(playerTeam[1, 0], encounter.instantiatedEnemyConfiguration[0]),
                       "The turns should not be valid, as " + playerTeam[1, 0] + "'s use of their instrument is not valid.");
        Assert.IsTrue(testFlow.TurnsTaken == 0,
                      "The flow should not be progessed as the turn was invalid.");

    }
   
    private void EmptyOnTeamWinDelegate(int team) { }

}

