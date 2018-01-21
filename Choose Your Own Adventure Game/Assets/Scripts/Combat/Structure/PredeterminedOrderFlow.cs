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
            Assert.AreEqual(avaliableUnits.Count, 1, "Precondition Fail: There should only be one avaliable unit.");
            Assert.IsTrue(new List<Unit>(avaliableUnits).TrueForAll(unit => unit.health > 0),
                          "Precondition Fail: No avaliable unit should have health 0 or less.");
            Assert.IsNotNull(teamsAndOrders, "Precondition Fail: The argument 'teamsAndOrders' should not be null.");
            Assert.IsTrue(teamsAndOrders.Count > 0, "Precondition Fail: 'teams and orders should have at least one team.'");
            Assert.IsTrue(new List<LinkedList<Unit>>(teamsAndOrders).TrueForAll(list => list.Count > 0),
                          "Precondition Fail: Every team in teams and orders should have at least one member.");
            Assert.IsFalse(new List<LinkedList<Unit>>(teamsAndOrders).TrueForAll(
                t => new List<Unit>(t).TrueForAll( unit => unit.health <= 0 )
            ), "Precondition Fail: There should be at least one unit with more than 0 health.");


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


            //Create a new turn with the new team data.
            int nextTeam = Team + 1 < teamsAndOrders.Count ? Team + 1 : 0;
            while (teamNextUnitNodes[nextTeam].Value.health <= 0)
                nextTeam = Team + 1 < teamsAndOrders.Count ? Team + 1 : 0;

            Turn newTurn = new AlternatingPredeterminedOrderTurn(nextTeam, new HashSet<Unit>(new Unit[] { teamNextUnitNodes[nextTeam].Value }),
                                                                 teamsAndOrders);


            //Find the next node with a unit in the team that had more than 0 health, or set it to the same node if node exists.
            LinkedListNode<Unit> currentNode = teamNextUnitNodes[Team];
            LinkedListNode<Unit> newNode = currentNode.Next == null ? currentNode.List.First : currentNode.Next;
            while ( newNode != currentNode && newNode.Value.health == 0)
                newNode = newNode.Next == null ? newNode.List.First : newNode.Next;

            teamNextUnitNodes[Team] = newNode;


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
            Assert.IsFalse(teamNextUnitNodes[nextTeam].Value.health <= 0,
                           "Postcondition Fail: The avaliable unit for next turn should not have 0 health.");

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
