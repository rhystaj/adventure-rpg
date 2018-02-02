using System.Collections.Generic;

public interface ICombatEncounter
{
    int columnsPerSide { get; set; }
    List<IUnit> enemyConfiguration { get; }
    int rows { get; set; }
}