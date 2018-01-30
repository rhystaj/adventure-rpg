using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * The back-end of combat.
 */
public class CombatScenario<F> where F : CombatFlow
{

    private Once<Unit[,]> board; //The grid of units.
    private Once<CombatFlow> flow; //The struture of the progression of the combat.

    public CombatScenario(Unit[,] playerUnits, CombatEncounter encounter, CombatFlow.Adaptor flowAdaptor)
    {

        //Preconditions
        Assert.IsTrue(playerUnits.GetLength(0) == encounter.rows && playerUnits.GetLength(1) == encounter.columnsPerSide,
                      "Precondition Fail: The player configuration should have the same dimensions and the enemy configuration.");
        Assert.IsNotNull(encounter, "Precondition Fail: The argument 'encounter' should not be null.");
        Assert.IsNotNull(flowAdaptor, "Precondition Fail: The argument 'flowAdaptor' should not be null.");


        //Create a new board
        board.Value = new Unit[encounter.rows, encounter.columnsPerSide * 2]; //Double as columns per side is the number of units on ONE of the TWO sides.


        //Copy the enemy units from the encounter to the enemy side.
        for (int i = 0; i < board.Value.GetLength(0); i++)
        {
            for (int j = encounter.columnsPerSide; j < board.Value.GetLength(1); j++)
            {
                Unit enemy = encounter.enemyConfiguration[(j - encounter.columnsPerSide) + (encounter.columnsPerSide * i)];
                board.Value[i, j] = Object.Instantiate(enemy) as Unit;
            }
        }

        flow.Value = flowAdaptor.Convert(playerUnits, encounter);

        //Postconditions
        Assert.IsNotNull(board, "Postcondition Fail: The field 'board' should not be null.");
        Assert.IsTrue(board.Value.Length == encounter.columnsPerSide * encounter.rows * 2);
        Assert.IsNotNull(flow, "Postcondition Fail: The field 'flowAdaptor' should not be null.");

    }

}