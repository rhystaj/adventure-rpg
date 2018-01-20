using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

/**
 * A flow in which the teams alternate unit by unit, and the order in which units are allowed to move is predetermined.
 */
public class PredeterminedOrderFlow : CombatFlow {

    public PredeterminedOrderFlow(LinkedList<LinkedList<Unit>> teamsAndOrders)
    {

        //Preconditions
        Assert.IsNotNull(teamsAndOrders, "Precondition Fail: The argument 'teamsAndOrders' should not be null.");
        Assert.IsTrue(teamsAndOrders.Count > 0, "Precondition Fail: 'teams and orders should have at least one team.'");
        Assert.IsTrue(new List<LinkedList<Unit>>(teamsAndOrders).TrueForAll(list => list.Count > 0),
                      "Precondition Fail: Every team in teams and orders should have at least one member.");


        //Create an initial turn with the first team and unit.
        currentTurn = new AlternatingPredeterminedOrderTurn(0, new HashSet<Unit>(new Unit[] { teamsAndOrders.First.Value.First.Value }), teamsAndOrders);


        //Postconditions
        Assert.IsNotNull(currentTurn, "Postcondition Fail: The inherited field 'currentTurn' should not be null.");

    }

    private class AlternatingPredeterminedOrderTurn : Turn
    {

        private LinkedList<LinkedList<Unit>> teamsAndOrders; //The teams and the order in which thier units are allowed to move.
        private LinkedListNode<Unit>[] teamNextUnitNodes; //The units from each team that will nest move on the team's turn.

        //Assertion Fields
        private LinkedList<LinkedList<Unit>> teamsAndOrdersOnConstruction;
        private LinkedListNode<Unit>[] teamIndexesOnConstruction;

        private LinkedList<Unit> currentTeamUnits;


        public AlternatingPredeterminedOrderTurn(int team, HashSet<Unit> avaliableUnits, LinkedList<LinkedList<Unit>> teamsAndOrders) : base(team, avaliableUnits) {

            //Preconditions
            Assert.IsNotNull(teamsAndOrders, "Precondition Fail: The argument 'teamsAndOrders' should not be null.");
            Assert.IsTrue(teamsAndOrders.Count > 0, "Precondition Fail: 'teams and orders should have at least one team.'");
            Assert.IsTrue(new List<LinkedList<Unit>>(teamsAndOrders).TrueForAll(list => list.Count > 0),
                          "Precondition Fail: Every team in teams and orders should have at least one member.");


            this.teamsAndOrders = teamsAndOrders;
            teamNextUnitNodes = new LinkedListNode<Unit>[teamsAndOrders.Count];


            //Assertion only setup
            Assert.IsTrue(RecordFinalVariables());


            //Postconditions
            Assert.IsTrue(ClassInvaraintsHold());
            Assert.IsNotNull(teamsAndOrders, "Postcondition Fail: The argument 'teamsAndOrders' should not be null.");
            Assert.IsNotNull(teamNextUnitNodes, "Postcondition Fail: The argument 'teamIndexes' should not be null.");
            

        }

        public override Turn take(Unit subject)
        {

            //Preconditions
            Assert.IsNotNull(subject, "Precondition Fail: The argument 'subject' should not be null.");


            //Assertion value tracking
            LinkedListNode<Unit> currentNode = teamNextUnitNodes[Team];


            //Create a new turn with the new team data.
            int nextTeam = Team + 1 < teamsAndOrders.Count ? Team + 1 : 0; 
            Turn newTurn = new AlternatingPredeterminedOrderTurn(nextTeam, new HashSet<Unit>(new Unit[] { teamNextUnitNodes[nextTeam].Value }),
                                                                 teamsAndOrders);


            //Set the current team's unit to the next unit (or first if the end of the list is reached) for the next time it is the team's turn.
            teamNextUnitNodes[Team] = teamNextUnitNodes[Team].Next == null ? teamNextUnitNodes[Team].List.First : teamNextUnitNodes[Team].Next; 


            //Postconditions
            Assert.IsTrue(ClassInvaraintsHold());
            Assert.IsTrue(newTurn.Team >= 0 && newTurn.Team < teamsAndOrders.Count,
                          "Postcondition Fail: The next team number (" + newTurn.Team + ") should be valid - i.e greater at least 0, and smaller than the " +
                          "number of teams (" + teamsAndOrders.Count + ").");
            Assert.IsTrue(Team < teamsAndOrders.Count - 1 || newTurn.Team == 0,
                          "Postcondition Fail: If the previous team's number was the higest, the current team should be 0)");
            Assert.IsTrue(Team == teamsAndOrders.Count - 1 || newTurn.Team == Team + 1,
                          "Postcondition Fail: If the previous team's number wasn't the higest, the current team should be one more than the previous.");
            Assert.IsTrue(currentNode.Next == null || teamNextUnitNodes[Team] == currentNode.Next,
                          "Postcondition Fail: The node for the current team should now be the node with the team's next unit, if the current node" +
                           "has none.");
            Assert.IsTrue(currentNode.Next != null || teamNextUnitNodes[Team] == currentNode.List.First);

            return newTurn;

        }


        //Assertion methods
        private bool RecordFinalVariables()
        {

            teamsAndOrdersOnConstruction = teamsAndOrders;
            teamIndexesOnConstruction = teamNextUnitNodes;

            return true;

        }

        private bool ClassInvaraintsHold()
        {

            Assert.IsTrue(teamsAndOrders == teamsAndOrdersOnConstruction,
                          "Postcondition Fail: The object referenced by 'teamsAndOrders' should not be changed at runtime.");
            Assert.IsTrue(teamNextUnitNodes == teamIndexesOnConstruction,
                          "Postcondition Fail: The object referenced by 'teamIndexes' should not be changed runtime.");

            return true;

        }

    }

}
