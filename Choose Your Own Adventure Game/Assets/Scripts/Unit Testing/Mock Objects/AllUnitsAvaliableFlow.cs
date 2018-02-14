using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUnitsAvaliableFlow : CombatFlow {

	public AllUnitsAvaliableFlow(HashSet<Unit.IInstance> units)
    {
        currentTurn = new AllUnitsTurn(0, units);
    }

    protected class AllUnitsTurn : Turn
    {

        private HashSet<Unit.IInstance> units;

        public AllUnitsTurn(int team, HashSet<Unit.IInstance> avaliableUnits) : base(team, avaliableUnits)
        {
            Debug.Log("Given Turn: " + team);
            units = avaliableUnits;
        }
    
        protected override Turn ProduceNextTurn(Unit.IInstance turnTaker)
        {
            return new AllUnitsTurn(Team == 0 ? 1 : 0, units);
        }
    }

    public class AllAdaptor : Adaptor
    {
        public CombatFlow Convert(Unit.IInstance[,] playerTeam, ICombatEncounter encounter)
        {

            HashSet<Unit.IInstance> units = new HashSet<Unit.IInstance>();

            units.UnionWith(TestingUtil.Convert2DArrayToList(playerTeam));
            units.UnionWith(encounter.instantiatedEnemyConfiguration);

            return new AllUnitsAvaliableFlow(units);

        }
    }

}
