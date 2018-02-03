using System.Collections.Generic;

public interface ICombatEncounter
{
    int columnsPerSide { get; set; }
    List<Unit.IInstance> instantiatedEnemyConfiguration { get; }
    int rows { get; set; }
}