using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * The settings for a particular combat scenario.
 */
[CreateAssetMenu]
public class CombatEncounter : ScriptableObject
{

    [Header("Dimensions")]
    public int columnsPerSide; //The number of columns on each of the TWO sides of the board, i.e. the actual number of colums will be double.
    public int rows; //The number of rows the board has.

    [HideInInspector]
    public List<Unit> enemyConfiguration = new List<Unit>(); //The positioning of the units in the enemy team.

}
