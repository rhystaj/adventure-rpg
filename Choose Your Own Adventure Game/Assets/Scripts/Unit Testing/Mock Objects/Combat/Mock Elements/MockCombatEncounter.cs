using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockCombatEncounter : ICombatEncounter
{

    private int _columnsPerSide;
    private int _rows;
    private List<Unit.IInstance> _enemyConfiguration;

    public int columnsPerSide { get { return _columnsPerSide; } set { columnsPerSide = value; } }
    public int rows { get { return _rows; } set { _columnsPerSide = value; } }
    public List<Unit.IInstance> instantiatedEnemyConfiguration { get { return _enemyConfiguration; } }

    public MockCombatEncounter(int columnsPerSide, int rows, List<Unit.IInstance> enemyConfiguration)
    {

        _columnsPerSide = columnsPerSide;
        _rows = rows;
        _enemyConfiguration = enemyConfiguration;

    }
    
}
