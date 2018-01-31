using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * The settings for a particular combat scenario.
 */
[CreateAssetMenu]
public class CombatEncounter : ScriptableObject, ICombatEncounter
{

    [Header("Dimensions")]
    [SerializeField] int _columnsPerSide; //The number of columns on each of the TWO sides of the board, i.e. the actual number of colums will be double.
    [SerializeField] int _rows; //The number of rows the board has.

    [HideInInspector]
    private List<IUnit> _enemyConfiguration = new List<IUnit>(); //The positioning of the units in the enemy team.


    //Public getters.
    public int columnsPerSide { get { return _columnsPerSide; } set { _columnsPerSide = value; } }
    public int rows { get { return _rows; } set { _rows = value; } }
    public List<IUnit> enemyConfiguration { get { return _enemyConfiguration; } }

}
