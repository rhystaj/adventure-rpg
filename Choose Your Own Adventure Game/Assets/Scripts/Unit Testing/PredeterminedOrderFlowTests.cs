using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NSubstitute;
using System.Collections.Generic;
using UnityEditor;

public class PredeterminedOrderFlowTests : MonoBehaviour {

    private LinkedList<LinkedList<Unit>> testUnits = new LinkedList<LinkedList<Unit>>();

    [OneTimeSetUp]
    public void PrepareForTests()
    {

        //The default mock team setup.
        List<List<Unit>> mockTeams = new List<List<Unit>>( new List<Unit>[] {
            new List<Unit>( new Unit[]{
                CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 1"),
                CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 2"),
                CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 3")
            }),

            new List<Unit>( new Unit[] {
               CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 4"),
               CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 5"),
               CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 6")
            }),

            new List<Unit>( new Unit[]{
                CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 7"),
                CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 8"),
                CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 9"),
                CombatFlowTests.GetFlowTestUnit("Flow Test Mock Unit 10")
            }),

        });


        //Put mock teams into testUnits.
        for (int i = 0; i < mockTeams.Count; i++)
        {

            LinkedList<Unit> currentTeam = new LinkedList<Unit>();

            for (int j = 0; j < mockTeams[i].Count; j++)
                currentTeam.AddLast(mockTeams[i][j]);

            testUnits.AddLast(currentTeam);

        }

        Debug.Log("testUnits.Count: " + testUnits.Count);


    }

    /**
     * Ensures that the team number is either incremented or looped back to 0 after the highest number whenever a turn is taken.
     */ 
	[Test]
    public void TeamNumberIsIncrementedAndLooped()
    {

        PredeterminedOrderFlow flow = new PredeterminedOrderFlow(testUnits);

        for(int i = 0; i < 100; i++)
        {

            int teamBefore = flow.CurrentTeam;

            flow.TakeTurn(new List<Unit>(flow.UnitsAvaliableForTurn)[0]);

            if (teamBefore == testUnits.Count - 1)
                Assert.IsTrue(flow.CurrentTeam == 0, "The team number should loop back to 0 when the max is reached.");
            else
                Assert.IsTrue(flow.CurrentTeam == teamBefore + 1,
                              "The team number should be incorrected, provided it does not exceed the number of teams.");

        }

    }

    //[Test]
    public void CorrectUnitsAreSelectedAtTheRightTime()
    {

        CombatFlow flow = new PredeterminedOrderFlow(testUnits);

        foreach(LinkedList<Unit> team in testUnits)
        {



        }

    }

 
    [TearDown]
    public void ResetHealthOfAllUnits()
    {

        foreach (LinkedList<Unit> team in testUnits)
            foreach (Unit unit in team) unit.health = unit.maxHealth;

    }
    



}
