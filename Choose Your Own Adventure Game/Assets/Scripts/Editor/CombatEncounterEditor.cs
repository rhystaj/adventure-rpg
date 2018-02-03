using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * The custom inspector for CombatScenario
 */
[CustomEditor(typeof(CombatEncounter))]
public class CombatEncounterEditor : Editor
{

    const int UNIT_FIELD_WIDTH = 100;

    private CombatEncounter targetScenario; //The CombatScenario being modified by the editor.

    private void OnEnable()
    {
        targetScenario = (CombatEncounter)target;
    }

    override public void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        //Ensure the dimension can't have negative values.
        if (targetScenario.rows < 0) targetScenario.rows = 0;
        if (targetScenario.columnsPerSide < 0) targetScenario.columnsPerSide = 0;

        //Adjust the length of targetScenario.enemyConfiguration if the number of rows and columns have changed.
        while (targetScenario.rawEnemyConfiguration.Count < targetScenario.rows * targetScenario.columnsPerSide)
            targetScenario.rawEnemyConfiguration.Add(null);
        while (targetScenario.rawEnemyConfiguration.Count > targetScenario.rows * targetScenario.columnsPerSide)
            targetScenario.rawEnemyConfiguration.Remove(null);


        GUILayout.Space(10);
        DrawEnemyUnitPositionSelection();

    }

    /**
     * Configure the part of the editor that would allow you to set the position of the enemy units.
     */
    private void DrawEnemyUnitPositionSelection()
    {

        EditorGUILayout.LabelField("Enemy Positions", EditorStyles.boldLabel);

        int i = 0;
        for (int row = 0; row < targetScenario.rows; row++)
        {

            GUILayout.BeginHorizontal();

            for (int start = row * targetScenario.columnsPerSide; i - start < targetScenario.columnsPerSide; i++)
                targetScenario.rawEnemyConfiguration[i] = (Unit)EditorGUILayout.ObjectField(targetScenario.rawEnemyConfiguration[i],
                                                                                   typeof(Unit), false, GUILayout.Width(UNIT_FIELD_WIDTH));

            GUILayout.EndHorizontal();

        }

    }

}
