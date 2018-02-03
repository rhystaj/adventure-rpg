using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LastTeamStandingWinTracker : CombatScenario.WinTracker
{
    public int DetermineWinner(Unit.IInstance[,] board)
    {

        //Preconditions
        Assert.IsNotNull(board, "Precondition Fail: The argument 'board' should not be null.");
        Assert.IsTrue(TestingUtil.Convert2DArrayToList(board).TrueForAll(u => u.alignment != -1),
                      "Precondition Fail: The argument 'board' should not contain a unit with alignment -1.");


        //Variables for assertions.
        HashSet<int> teamNumbers = new HashSet<int>();


        //Units on the board into thier teams, so they can be analysed as a whole team.
        List<List<Unit.IInstance>> teams = new List<List<Unit.IInstance>>();

        foreach (Unit.IInstance u in board) {

            //Ensure u is a valid unit, and add it to the set of valid team numbers.
            if (u == null) continue;
            teamNumbers.Add(u.alignment);


            //Ensure there is enough space for a unit to be put at the index of thier alignment, then add it there.
            while (u.alignment >= teams.Count) teams.Add(new List<Unit.IInstance>());

            Assert.IsTrue(u.alignment < teams.Count,
                          "Enough elements should be added to teams, that the units aignment should be a valid index.");

            teams[u.alignment].Add(u);

        }


        //Count the number of teams with all units on 0 health, and record the other teams.
        int defeatedTeams = 0;
        HashSet<int> standingTeams = new HashSet<int>();

        for(int team = 0; team < teams.Count; team++)
        {

            Assert.IsNotNull(teams[team], "There should be no null elements in 'teams'");

            if (teams[team].TrueForAll(u => u.health <= 0))
                defeatedTeams++;
            else standingTeams.Add(team);

        }


        //If the number of defated teams is one less than the total number of teams, the standing team is the winner.
        int result;

        if (defeatedTeams == teams.Count - 1)
        {

            Assert.IsTrue(standingTeams.Count == 1,
                          "If there is 1 less defeated team than the number of teams, there should only be one standing team.");

            result = new List<int>(standingTeams)[0]; //Select the first unit, as there is only one anyway.

        }
        else if (standingTeams.Count == 0) throw new NoTeamStandingException(); //If there are no teams standing, something may have wrong.
        else result = -1; //We don't have a winner otherwise.


        //Postcondition
        Assert.IsTrue(result == -1 || teamNumbers.Contains(result),
                      "Postcondition Fail: If the result (" + result + ") is not -1, it should be a valid team number:" +
                       TestingUtil.PrintsItemsOf(teamNumbers));
        Assert.IsTrue(!standingTeams.Contains(-1),
                      "Postcondition Fail: There should not be a valid team with alignment -1");

        return result;

    }

    public void Update(Unit.IInstance[,] board){ /* Not applicable */ }

    public class NoTeamStandingException : Exception { }

}
