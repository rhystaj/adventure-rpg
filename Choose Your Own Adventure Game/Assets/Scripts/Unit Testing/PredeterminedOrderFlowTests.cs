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

    private IUnit[] testUnits;
    

    private LinkedList<LinkedList<IUnit>> testTeams = new LinkedList<LinkedList<IUnit>>();

    [OneTimeSetUp]
    public void PrepareForTests()
    {

        //Load in test units.
        testUnits = new IUnit[] {
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
        List<List<IUnit>> mockTeams = new List<List<IUnit>>( new List<IUnit>[] {
            new List<IUnit>( new IUnit[] {
                testUnits[0],
                testUnits[1],
                testUnits[2]
            }),

            new List<IUnit>( new IUnit[] {
                testUnits[3],
                testUnits[4],
                testUnits[5],
            }),

            new List<IUnit>( new IUnit[] {
                testUnits[6],
                testUnits[7],
                testUnits[8],
                testUnits[9]
            }),

        });


        //Put mock teams into testUnits.
        for (int i = 0; i < mockTeams.Count; i++)
        {

            LinkedList<IUnit> currentTeam = new LinkedList<IUnit>();

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

            flow.TakeTurn(new List<IUnit>(flow.UnitsAvaliableForTurn)[0]);

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

            flow.TakeTurn(new List<IUnit>(flow.UnitsAvaliableForTurn)[0]);

        }

    }

    /**
     * Ensures that units are made avaliable when they should.
     */ 
    [Test]
    public void CorrectUnitsAreSelectedAtTheRightTime()
    {

        CombatFlow flow = new PredeterminedOrderFlow(testTeams);

        TestUnitsAreSelectedInOrder(flow, new int[] { 1, 4, 7, 2, 5, 8, 3, 6, 9, 1, 4, 10 });

    }

    /**
     * Ensures that a unit, somewhere in the team that is not first or last, is skipped when thier health is equal to 0.
     */ 
    [Test]
    public void UnitWith0HealthInMiddleOfTeamIsSkipped()
    {
        TestUnitSkip(new int[] { 4 }, 4, 5);
    }

    /**
     * Ensures that a unit, at the beginning of the team, is skipped when thier health is equal to 0.
     */
    [Test]
    public void UnitWith0HealthAtTheStartOfTeamIsSkipped()
    {
        TestUnitSkip(new int[] { 6 }, 2, 7);
    }

    /**
     * Ensures that a unit, at the end of the team, is skipped when thier health is equal to 0.
     */
    [Test]
    public void UnitWith0HealthAtTheEndOfTeamIsSkipped()
    {
        TestUnitSkip(new int[] { 2 }, 6, 0);
    }

    /**
     * Ensures that a team with no units with health is skipped.
     */
    [Test]
    public void ATeamIsSkippedWhenNoneOfItsUnitsHaveHealth()
    {
        TestUnitSkip(new int[] { 3, 4, 5 }, 1, 6);
    }

    /**
     * Ensures that if a unit's health reaches 0, and is then restored, the unit won't be skipped.
     */ 
    [Test]
    public void AUnitWithItsHealthRestoredIsntStillSkipped()
    {

        CombatFlow flow = TestUnitSkip(new int[] { 4 }, 4, 5);

        testUnits[4].health = 50;
        for(int i = 0; i < 6; i++) flow.TakeTurn(new List<IUnit>(flow.UnitsAvaliableForTurn)[0]);
        Assert.IsTrue(flow.UnitsAvaliableForTurn.Contains(testUnits[4]),
                testUnits[4] + " should be avaliable. \n" +
                          "Avaliable Units: " + String.Format(TestingUtil.PrintsItemsOf(flow.UnitsAvaliableForTurn)));

    }

    /**
     * Ensures that if a unit only has one team member wil more than 0 health, that unit will keep being selected upon the team's turn.
     */
     [Test]
     public void ATeamWithOnlyOneStandingUnitWillConstantyHaveThatUnitSelectedOnThierTurn()
    {

        CombatFlow flow = TestUnitSkip(new int[] { 3, 5 }, 1, 4);
        for (int i = 0; i < 100; i++) TestUnitSkip(flow, new int[] { }, 3, 4);

    }

    /**
     * Ensures that if the first unit of the first team has a health of 0 initially, it is skipped.
     */ 
    [Test]
    public void FlowDoesNotStartWithUnitWith0Health()
    {
        testUnits[0].health = 0;
        TestUnitSkip(new int[] { 0 }, 0, 1);
    }

    /**
     * Ensures that all the units in the first team have 0 health, the flow automatically starts with the second team.
     */ 
    [Test]
    public void FlowDoesNotStartWithATeamWithNoStandingUnits()
    {
        testUnits[0].health = 0;
        testUnits[1].health = 0;
        testUnits[2].health = 0;
        TestUnitSkip(new int[] { 0, 1, 2 }, 0, 3);
    }

    /**
     * Ensures that if a unit only has one team member wil more than 0 health, that unit will keep being selected upon the team's turn.
     */
    [Test]
    public void AFlowWithOnlyOneUnitWillConstantlySelectThatUnit()
    {

        for (int i = 0; i < 100; i++) ;
            CombatFlow flow = TestUnitSkip(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 1, 0);

    }

    /**
     * Ensures that if there are no units left to move, the correct error is thrown.
     */ 
    [Test]
    public void NoValidTurnExceptionIsThrownIfThereAreNoStandingUnits()
    {

        CombatFlow flow = new PredeterminedOrderFlow(testTeams);
        flow.TakeTurn(new List<IUnit>(flow.UnitsAvaliableForTurn)[0]);

        foreach (IUnit unit in testUnits) unit.health = 0;

        try
        {
            flow.TakeTurn(new List<IUnit>(flow.UnitsAvaliableForTurn)[0]);
            Assert.Fail("An error should be thrown when all units in a flow have 0 health.");
        }
        catch (CombatFlow.NoValidNextTurnException e){ }

    }


    [Test]
    public void PDOAdaptorCreatesCorrectFlow()
    {

        //The mock teams to be given to the adaptor.
        IUnit[,] playerTeam = new IUnit[,] { { testUnits[0], testUnits[2] }, { testUnits[3], testUnits[1] } };
        IUnit[] enemyTeam = new IUnit[] { testUnits[4], testUnits[7], testUnits[5], testUnits[6] };


        //Create CombatEncounter to be passed into Adaptor.
        ICombatEncounter encounter = new MockCombatEncounter(2, 2, new List<IUnit>(enemyTeam));


        //Specify the order in which the units will be selected on the player and enemy teams;
        Vector2[] playerOrder = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 1)
        };

        Vector2[] enemyOrder = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };


        //Produce flow unsing adaptor and test.
        PredeterminedOrderFlow.PDOAdaptor adaptor = new PredeterminedOrderFlow.PDOAdaptor(playerOrder, enemyOrder);
        PredeterminedOrderFlow flow = adaptor.Convert(playerTeam, encounter) as PredeterminedOrderFlow;

        TestUnitsAreSelectedInOrder(flow, new int[] { 1, 5, 2, 6, 3, 7, 4, 8 });

    }


    [TearDown]
    public void ResetHealthOfAllUnits()
    {

        foreach (LinkedList<IUnit> team in testTeams)
            foreach (Unit unit in team) unit.health = unit.maxHealth;

    }

    /**
     * Ensures that the given flow selected the units in the given order.
     */ 
    private void TestUnitsAreSelectedInOrder(CombatFlow flow, int[] mockTestUnitOrder)
    {

        foreach (int unitNumber in mockTestUnitOrder)
        {

            IUnit expectedUnit = testUnits[unitNumber - 1];
            Assert.IsTrue(flow.UnitsAvaliableForTurn.Contains(expectedUnit),
                          expectedUnit + " should be avaliable. \n" +
                          "Avaliable Units: " + String.Format(TestingUtil.PrintsItemsOf(flow.UnitsAvaliableForTurn)));

            flow.TakeTurn(new List<IUnit>(flow.UnitsAvaliableForTurn)[0]);

        }

    }

    /**
     * Skips the given units a newly created flow and check that the givenExpected unit is the current unit after the given amount of turns.
     */
    private CombatFlow TestUnitSkip(int[] numbersToSkip, int turnsToTake, int expectedUnit)
    {

        CombatFlow flow = new PredeterminedOrderFlow(testTeams);
        return TestUnitSkip(flow, numbersToSkip, turnsToTake, expectedUnit);

    }

    /**
     * Skips the given units in the given flow, and check that the givenExpected unit is the current unit after the given amount of turns.
     */ 
    private CombatFlow TestUnitSkip(CombatFlow flow, int[] numbersToSkip, int turnsToTake, int expectedUnit)
    {

        foreach (int num in numbersToSkip) testUnits[num].health = 0;

        for (int i = 0; i < turnsToTake; i++) flow.TakeTurn(new List<IUnit>(flow.UnitsAvaliableForTurn)[0]);
        Assert.IsTrue(flow.UnitsAvaliableForTurn.Contains(testUnits[expectedUnit]),
               testUnits[expectedUnit] + " should be avaliable. \n" +
                          "Avaliable Units: " + String.Format(TestingUtil.PrintsItemsOf(flow.UnitsAvaliableForTurn)));

        return flow;

    }





}
