using System.Collections.Generic;

/**
 * A combat flow that has each team able to move one of thier units once, before the other moves.
 * Most likey will not be used.
 */ 
public class TeamByTeamFlow : CombatFlow {

    List<List<Unit>> teams;

	public TeamByTeamFlow(int startingTeam, List<List<Unit>> teams)
    {
        this.teams = new List<List<Unit>>(teams);
        currentTurn = new TeamByTeamTurn(startingTeam, new HashSet<Unit>(teams[startingTeam]), teams);
    }

    protected class TeamByTeamTurn : Turn
    {

        private List<List<Unit>> teams;

        public TeamByTeamTurn(int team, HashSet<Unit> avaliableUnits, List<List<Unit>> teams) : base(team, avaliableUnits) {
            this.teams = new List<List<Unit>>(teams);
        }

        protected override Turn ProduceNextTurn(Unit subject)
        {

            if (!CanMove(subject)) return this; //Do nothing if the unit is not able to move.

            //Remove the unit that has just moved from the list of avaliable units.
            HashSet<Unit> nextAvaliableUnits = new HashSet<Unit>(AvaliableUnits);
            nextAvaliableUnits.Remove(subject);

            if (nextAvaliableUnits.Count == 0)
            {
                //If there are no more units on the current team, it is the next team's turn to move.
                int nextTeam = (Team + 1) % teams.Count;
                return new TeamByTeamTurn(nextTeam, new HashSet<Unit>(teams[nextTeam]), teams);
            }
            else
            {
                //Otherwise everyone on that has not moved on the current team can still move.
                return new TeamByTeamTurn(Team, nextAvaliableUnits, teams);
            }


        }
    }

}
