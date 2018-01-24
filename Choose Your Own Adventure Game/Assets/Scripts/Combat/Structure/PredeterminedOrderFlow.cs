using System.Collections.Generic;
using UnityEngine;
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

        Debug.Log("Constructor");

        //Record the first unit of each team.
        LinkedListNode<Unit>[] teamNextUnitNodes = new LinkedListNode<Unit>[teamsAndOrders.Count];

        int i = 0;
        foreach (LinkedList<Unit> team in teamsAndOrders)
        { 
            teamNextUnitNodes[i] = team.First;
            i++;
        }


        Debug.Log("teamNextUnitNodes: " + TestingUtil.PrintsItemsAs(teamNextUnitNodes, n => n.Value.ToString() ));

        //Create an initial turn with the first team and unit.
        currentTurn = new AlternatingPredeterminedOrderTurn(0, new HashSet<Unit>(new Unit[] { teamsAndOrders.First.Value.First.Value }), 
                                                            teamsAndOrders, teamNextUnitNodes);


        //Postconditions
        Assert.IsNotNull(currentTurn, "Postcondition Fail: The inherited field 'currentTurn' should not be null.");
        Assert.IsTrue(TeamNextUnitNodesHasTheFirstUnitOfEachTeamInOrder(teamNextUnitNodes, teamsAndOrders),
                          "Postcondition Fail: teamNextUnitNodes should have the first node of each of the teams, in order.");


    }

    private class AlternatingPredeterminedOrderTurn : Turn
    {

        private LinkedList<LinkedList<Unit>> teamsAndOrders; //The teams and the order in which thier units are allowed to move.
        private LinkedListNode<Unit>[] teamNextUnitNodes; //The units from each team that will nest move on the team's turn.

        //Assertion Fields
        private LinkedList<LinkedList<Unit>> teamsAndOrdersOnConstruction;
        private LinkedListNode<Unit>[] teamIndexesOnConstruction;

        private LinkedList<Unit> currentTeamUnits;


        public AlternatingPredeterminedOrderTurn(int team, HashSet<Unit> avaliableUnits, LinkedList<LinkedList<Unit>> teamsAndOrders,
                                                 LinkedListNode<Unit>[] teamNextUnitNodes) : base(team, avaliableUnits) {

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
            ), "Precondition Fail: There should be at least one unit int teams and orders with more than 0 health.");


            this.teamsAndOrders = teamsAndOrders;
            this.teamNextUnitNodes = teamNextUnitNodes;


            //Assertion only setup
            Assert.IsTrue(RecordFinalVariables());


            //Postconditions
            Assert.IsTrue(ClassInvaraintsHold());
            Assert.IsNotNull(teamsAndOrders, "Postcondition Fail: The argument 'teamsAndOrders' should not be null.");
            Assert.IsNotNull(teamNextUnitNodes, "Postcondition Fail: The argument 'teamNextUnitNodes' should not be null.");
            Assert.AreEqual(teamNextUnitNodes.Length, teamsAndOrders.Count,
                            "Postcondition Fail: teamNextUnitNodes should be the same length as teamsAndOrders.");
            

        }

        protected override Turn ProduceNextTurn(Unit turnTaker)
        {

            //Preconditions
            Assert.IsNotNull(turnTaker, "Precondition Fail: The argument 'subject' should not be null.");
            Assert.IsFalse(new List<LinkedList<Unit>>(teamsAndOrders).TrueForAll(
                t => new List<Unit>(t).TrueForAll(unit => unit.health <= 0)
            ), "Precondition Fail: There should be at least one unit int teams and orders with more than 0 health.");

            //Variable recording for assertions.
            LinkedList<LinkedList<Unit>> teamsAndOrdersAtStart;
            Assert.IsTrue(SetToCloneOfTeamsAndOrders(out teamsAndOrdersAtStart));


            //Determine the next team to go - i.e the next team with their next unit to move with more than 0 health..
            int nextTeam = Team + 1 < teamsAndOrders.Count ? Team + 1 : 0;
            while (teamNextUnitNodes[nextTeam].Value.health <= 0)
                nextTeam = Team + 1 < teamsAndOrders.Count ? Team + 1 : 0;


            //Find the next node with a unit in the team that had more than 0 health, or set it to the same node if node exists.
            LinkedListNode<Unit> currentNode = teamNextUnitNodes[Team];
            LinkedListNode<Unit> newNode = currentNode.Next == null ? currentNode.List.First : currentNode.Next;
            while ( newNode != currentNode && newNode.Value.health <= 0)
                newNode = newNode.Next == null ? newNode.List.First : newNode.Next;

            teamNextUnitNodes[Team] = newNode;

            Debug.Log("teamNextUnitNodes: " + TestingUtil.PrintsItemsAs(teamNextUnitNodes, n => n.Value.ToString()));


            //Postconditions
            Assert.IsTrue(ClassInvaraintsHold());
            Assert.IsTrue(nextTeam >= 0 && nextTeam < teamsAndOrders.Count,
                          "Postcondition Fail: The next team number (" + nextTeam + ") should be valid - i.e greater at least 0, and smaller than the " +
                          "number of teams (" + teamsAndOrders.Count + ").");
            Assert.IsTrue(Team < teamsAndOrders.Count - 1 || nextTeam == 0,
                          "Postcondition Fail: If the previous team's number was the higest, the current team should be 0)");
            Assert.IsTrue(Team == teamsAndOrders.Count - 1 || nextTeam == Team + 1,
                          "Postcondition Fail: If the previous team's number wasn't the higest, the current team should be one more than the previous.");
            Assert.IsTrue(currentNode.Next == null || teamNextUnitNodes[Team] == currentNode.Next,
                          "Postcondition Fail: The node for the current team should now be the node with the team's next unit, if the current node" +
                           "has none.");
            Assert.IsTrue(currentNode.Next != null || teamNextUnitNodes[Team] == currentNode.List.First);
            Assert.IsFalse(teamNextUnitNodes[nextTeam].Value.health <= 0,
                           "Postcondition Fail: The avaliable unit for next turn should not have 0 health.");
            Assert.IsTrue(TeamAndOrdersReaminsUnchanged(teamsAndOrdersAtStart),
                          "Precondition Fail: 'teamsAndOrders' should remain unchanged.");
            Assert.IsTrue(TestingUtil.CountItemsForWhichHolds(teamNextUnitNodes[Team].List, unit => unit.health < 0) <= 1 ||
                          teamNextUnitNodes[Team] != currentNode,
                          "Postcondition Fail: If there is more than one unit with more than 0 health in a team, the next node in that team to be made" +
                          "avaliable should be diferent.");

            return new AlternatingPredeterminedOrderTurn(nextTeam, new HashSet<Unit>(new Unit[] { teamNextUnitNodes[nextTeam].Value }),
                                                         teamsAndOrders, teamNextUnitNodes);

        }


        //Assertion methods
        private bool RecordFinalVariables()
        {

            teamsAndOrdersOnConstruction = teamsAndOrders;
            teamIndexesOnConstruction = teamNextUnitNodes;

            return true;

        }

        private bool SetToCloneOfTeamsAndOrders(out LinkedList<LinkedList<Unit>> capture)
        {

            capture = new LinkedList<LinkedList<Unit>>();
            foreach (LinkedList<Unit> team in teamsAndOrders)
                capture.AddLast(new LinkedList<Unit>(team));

            return true;

        }

        private bool TeamAndOrdersReaminsUnchanged(LinkedList<LinkedList<Unit>> teamsAndOrdersClone)
        {

            LinkedListNode<LinkedList<Unit>> currentTeamNode = teamsAndOrders.First;

            foreach(LinkedList<Unit> team in teamsAndOrders)
            {

                LinkedListNode<Unit> currentUnitNode = currentTeamNode.Value.First;

                foreach (Unit unit in team)
                {

                    if (unit != currentUnitNode.Value) return false;

                    currentUnitNode = currentUnitNode.Next;

                }

                currentTeamNode = currentTeamNode.Next;

            }

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

    private bool TeamNextUnitNodesHasTheFirstUnitOfEachTeamInOrder(LinkedListNode<Unit>[] teamNextUnitNodes, LinkedList<LinkedList<Unit>> teamsAndOrders)
    {

        LinkedListNode<LinkedList<Unit>> currentTeam = teamsAndOrders.First;

        foreach (LinkedListNode<Unit> node in teamNextUnitNodes)
        {

            if (node != currentTeam.Value.First) return false;

            currentTeam = currentTeam.Next;

        }

        return true;


    }

}
