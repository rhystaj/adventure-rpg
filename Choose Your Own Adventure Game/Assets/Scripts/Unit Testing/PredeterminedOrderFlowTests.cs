using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NSubstitute;
using System.Collections.Generic;

public class PredeterminedOrderFlowTests : MonoBehaviour {

    private LinkedList<LinkedList<Unit>> testUnits = new LinkedList<LinkedList<Unit>>();

    [OneTimeSetUp]
    public void PrepareForTests()
    {

        //The default mock team setup.
        Unit[,] mockTeams = new Unit[,]
        {
            {
                new Unit(Substitute.For<Instrument>(), 3, 3, 1),
                new Unit(Substitute.For<Instrument>(), 3, 2, 1),
                new Unit(Substitute.For<Instrument>(), 2, 3, 1)
            },

            {
                new Unit(Substitute.For<Instrument>(), 3, 3, 2),
                new Unit(Substitute.For<Instrument>(), 3, 2, 2),
                new Unit(Substitute.For<Instrument>(), 2, 3, 2)
            },

            {
                new Unit(Substitute.For<Instrument>(), 3, 3, 3),
                new Unit(Substitute.For<Instrument>(), 3, 2, 3),
                new Unit(Substitute.For<Instrument>(), 2, 3, 3)
            }

        };


        //Put mock teams into testUnits.
        for(int i = 0; i < mockTeams.GetLength(0); i++)
        {

            LinkedList<Unit> currentTeam = new LinkedList<Unit>();

            for (int j = 0; j < mockTeams.GetLength(1); j++)
                currentTeam.AddLast(mockTeams[i,j]);

            testUnits.AddLast(currentTeam);

        }

        Debug.Log("testUnits.Count: " + testUnits.Count);

    }

	[Test]
    public void TeamNumberIsIncrementedAndLooped()
    {

        PredeterminedOrderFlow flow = new PredeterminedOrderFlow(testUnits);

        for(int i = 0; i < 100; i++)
        {

            int teamBefore = flow.CurrentTeam;

            flow.TakeTurn(new List<Unit>(flow.UnitsAvaliableForTurn)[0]);

            if (teamBefore == 2)
                Assert.IsTrue(flow.CurrentTeam == 0, "Precondition Fail: The team number should loop back to 0 when the max is reached.");
            else
                Assert.IsTrue(flow.CurrentTeam == teamBefore + 1);

        }

    }

}
