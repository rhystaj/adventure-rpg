using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScenarioTests {

    private IUnit[,] playerTeam;
    private MockCombatEncounter encounter;


    [OneTimeSetUp][TearDown]
    public void GenerateTeams()
    {

        //Create test player team.
        playerTeam = new IUnit[,]
        {
            { new MockUnit("Player Unit 1", 0, 50, 4, true), new MockUnit("Player Unit 2", 0, 76, 7, true)},
            { new MockUnit("Player Unit 3", 0, 89, 3, true), new MockUnit("Player Unit 4", 0, 4, 2, true)}
        };


        //Create test enemy encounter.
        IUnit[] enemyTeam = new IUnit[]
        {
             new MockUnit("Enemy Unit 1", 1, 50, 4, true), new MockUnit("Enemy Unit 2", 1, 76, 7, true),
             new MockUnit("Enemy Unit 3", 1, 89, 3, true), new MockUnit("Enemy Unit 4", 1, 4, 2, true)
        };

        encounter = new MockCombatEncounter(2, 2, new List<IUnit>(enemyTeam));

    }


    /**
     * Ensures the copy of the board returned by the Board getter is correct.
     */ 
    [Test]
    public void CanGetCorrectCloneOfBoard()
    {

        IUnit[,] expectedArray = new IUnit[,]
        {
            { playerTeam[0,0], playerTeam[0,1], encounter.enemyConfiguration[0], encounter.enemyConfiguration[1] },
            { playerTeam[1,0], playerTeam[1,1], encounter.enemyConfiguration[2], encounter.enemyConfiguration[3] }
        };


        //Create the combat scenario and get its board.
        CombatScenario combatScenario = new CombatScenario(playerTeam, encounter,
                                                           new CombatFlow.DirectAdaptor(new TurnChangeTrackerMockCombatFlow(0, new IUnit[] { })),
                                                           new MockWinTracker(0, 5));
        IUnit[,] returned = combatScenario.Board;


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
                                                           new CombatFlow.DirectAdaptor(new TurnChangeTrackerMockCombatFlow(0, new IUnit[] { })),
                                                           new MockWinTracker(0, 5));
        IUnit[,] returned = combatScenario.Board;


        //Compare the positions of the units on the board with thier expected positions.
        //Compare the two arrays.
        for (int i = 0; i < returned.GetLength(0); i++)
            for (int j = 0; j < returned.GetLength(1); j++)
                Assert.AreEqual(correctPositions[i,j], returned[i, j].position,
                       "The unit " + returned[i, j] + " should have the position value " + correctPositions[i, j] + " " +
                       "instead it has the value " + returned[i, j].position + ".");

    }

}

