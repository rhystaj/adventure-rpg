using System.Collections.Generic;

/**
 * A combat flow that has each team able to move one of thier units once, before the other moves.
 * Most likey will not be used.
 */ 
public class TeamByTeamFlow : CombatFlow {

    List<List<Unit.IInstance>> teams;

	public TeamByTeamFlow(int startingTeam, List<List<Unit.IInstance>> teams)
    {
        this.teams = new List<List<Unit.IInstance>>(teams);
        currentTurn = new TeamByTeamTurn(startingTeam, new HashSet<Unit.IInstance>(teams[startingTeam]), teams);
    }

    protected class TeamByTeamTurn : Turn
    {

        private List<List<Unit.IInstance>> teams;

        public TeamByTeamTurn(int team, HashSet<Unit.IInstance> avaliableUnits, List<List<Unit.IInstance>> teams) : base(team, avaliableUnits) {
            this.teams = new List<List<Unit.IInstance>>(teams);
        }

        protected override Turn ProduceNextTurn(Unit.IInstance subject)
        {

            if (!CanMove(subject)) return this; //Do nothing if the unit is not able to move.

            //Remove the unit that has just moved from the list of avaliable units.
            HashSet<Unit.IInstance> nextAvaliableUnits = new HashSet<Unit.IInstance>(AvaliableUnits);
            nextAvaliableUnits.Remove(subject);

            if (nextAvaliableUnits.Count == 0)
            {
                //If there are no more units on the current team, it is the next team's turn to move.
                int nextTeam = (Team + 1) % teams.Count;
                return new TeamByTeamTurn(nextTeam, new HashSet<Unit.IInstance>(teams[nextTeam]), teams);
            }
            else
            {
                //Otherwise everyone on that has not moved on the current team can still move.
                return new TeamByTeamTurn(Team, nextAvaliableUnits, teams);
            }


        }
    }

}
