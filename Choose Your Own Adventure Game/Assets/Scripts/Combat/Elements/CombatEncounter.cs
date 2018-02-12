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
    [SerializeField] private List<Unit> _enemyConfiguration = new List<Unit>(); //The positioning of the units in the enemy team.
    private List<Unit.IInstance> _instantiatedEnemyConfiguration;


    //Public getters.
    public int columnsPerSide { get { return _columnsPerSide; } set { _columnsPerSide = value; } }
    public int rows { get { return _rows; } set { _rows = value; } }

    public List<Unit> rawEnemyConfiguration { get { return _enemyConfiguration; } }
    public List<Unit.IInstance> instantiatedEnemyConfiguration {
        get {
            if (_instantiatedEnemyConfiguration != null)
                return _instantiatedEnemyConfiguration;
            else
                throw new System.Exception("Can not call this getter before enable.");
        }
    }

    private void OnEnable()
    {

        

        _instantiatedEnemyConfiguration = _enemyConfiguration.ConvertAll<Unit.IInstance>(unit => new Unit.Instance(unit));

    }

}
