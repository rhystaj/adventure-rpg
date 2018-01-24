using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NSubstitute;
using System.Collections.Generic;
using UnityEditor;
using System;
using UnityScript.Lang;

public class PredeterminedOrderFlowTests : MonoBehaviour {

    private Unit[] testUnits;
    

    private LinkedList<LinkedList<Unit>> testTeams = new LinkedList<LinkedList<Unit>>();

    [OneTimeSetUp]
    public void PrepareForTests()
    {

        //Load in test units.
        testUnits = new Unit[] {
            CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 1"),
            CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 2"),
            CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 3"),
            CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 4"),
            CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 5"),
            CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 6"),
            CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 7"),
            CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 8"),
            CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 9"),
            CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 10")
        };


        //The default mock team setup.
        List<List<Unit>> mockTeams = new List<List<Unit>>( new List<Unit>[] {
            new List<Unit>( new Unit[] {
                testUnits[0],
                testUnits[1],
                testUnits[2]
            }),

            new List<Unit>( new Unit[] {
                testUnits[3],
                testUnits[4],
                testUnits[5],
            }),

            new List<Unit>( new Unit[] {
                testUnits[6],
                testUnits[7],
                testUnits[8],
                testUnits[9]
            }),

        });


        //Put mock teams into testUnits.
        for (int i = 0; i < mockTeams.Count; i++)
        {

            LinkedList<Unit> currentTeam = new LinkedList<Unit>();

            for (int j = 0; j < mockTeams[i].Count; j++)
                currentTeam.AddLast(mockTeams[i][j]);

            testTeams.AddLast(currentTeam);

        }

    }

    /**
     * Ensures that the team number is either incremented or looped back to 0 after the highest number whenever a turn is taken.
     */ 
	[Test]
    public void TeamNumberIsIncrementedAndLooped()
    {

        PredeterminedOrderFlow flow = new PredeterminedOrderFlow(testTeams);

        for(int i = 0; i < 100; i++)
        {

            int teamBefore = flow.CurrentTeam;

            flow.TakeTurn(new List<Unit>(flow.UnitsAvaliableForTurn)[0]);

            if (teamBefore == testTeams.Count - 1)
                Assert.IsTrue(flow.CurrentTeam == 0, "The team number should loop back to 0 when the max is reached.");
            else
                Assert.IsTrue(flow.CurrentTeam == teamBefore + 1,
                              "The team number should be incorrected, provided it does not exceed the number of teams.");

        }

    }

    /**
     * Ensures every turn only has one unit avaliable.
     */ 
    [Test]
    public void OnlyOneUnitIsEverAvaliable()
    {

        PredeterminedOrderFlow flow = new PredeterminedOrderFlow(testTeams);

        for (int i = 0; i < 100; i++)
        {

            int teamBefore = flow.CurrentTeam;

            Assert.AreEqual(1, flow.UnitsAvaliableForTurn.Count);

            flow.TakeTurn(new List<Unit>(flow.UnitsAvaliableForTurn)[0]);

        }

    }

    [Test]
    public void CorrectUnitsAreSelectedAtTheRightTime()
    {

        CombatFlow flow = new PredeterminedOrderFlow(testTeams);

        int[] mockTestUnitOrder = new int[] { 1, 4, 7, 2, 5, 8, 3, 6, 9, 1, 4, 10 };

        foreach(int unitNumber in mockTestUnitOrder)
        {

            Unit expectedUnit = testUnits[unitNumber - 1];
            Assert.IsTrue(flow.UnitsAvaliableForTurn.Contains(expectedUnit),
                          expectedUnit + " should be avaliable. /n" +
                          "Avaliable Units: " + String.Format(TestingUtil.PrintsItemsOf(flow.UnitsAvaliableForTurn)));

            flow.TakeTurn(new List<Unit>(flow.UnitsAvaliableForTurn)[0]);

        }

    }

 
    [TearDown]
    public void ResetHealthOfAllUnits()
    {

        foreach (LinkedList<Unit> team in testTeams)
            foreach (Unit unit in team) unit.health = unit.maxHealth;

    }
    



}
