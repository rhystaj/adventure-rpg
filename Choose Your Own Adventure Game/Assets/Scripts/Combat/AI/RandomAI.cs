using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * A controller that randomly moves an avaliable unit.
 */
public class RandomAI : IController
{
    public ICombatAction DetermineMove(Unit.IInstance[,] board, HashSet<Unit.IInstance> avaliableUnits)
    {

        //Preconditions
        Assert.IsNotNull(board, "Precondition Fail: The argument 'board' should not be null.");
        Assert.IsNotNull(board, "Precondition Fail: The argument 'avaliableUnits' should not be null.");
        Assert.IsFalse(avaliableUnits.Count == 0,
                       "Precondition Fail: The argument 'avaliableUnits' should not be empty.");
        Assert.IsTrue(new List<Unit.IInstance>(avaliableUnits).TrueForAll(unit => TestingUtil.Convert2DArrayToList(board).Contains(unit)),
                      "Precondition Fail: All avaliable units should be contained within board.");


        //Choose a random avaliable unit.
        int unitNumber = Random.Range(0, avaliableUnits.Count);
        Unit.IInstance choosenUser = new List<Unit.IInstance>(avaliableUnits)[unitNumber];


        //Choose a random valid unit to use thier instrument on.
        List<Unit.IInstance> validTargets = new List<Unit.IInstance>();
        foreach (Unit.IInstance unit in board) //Iterate over board finding units the choosen user can use thier instrument on.
            if (choosenUser.CanUseInstrumentOn(unit))
                validTargets.Add(unit);

        Assert.IsFalse(validTargets.Count == 0, "There should be at least on valid target for the choosen user.");

        unitNumber = Random.Range(0, validTargets.Count);
        Unit.IInstance chosenTarget = validTargets[unitNumber];


        //Set the action to the instrument use between the user and target.
        ICombatAction action = new InstrumentUse(choosenUser, chosenTarget);


        //Postconditions
        Assert.IsNotNull(action, "Postcondition Fail: The result should not be null.");

        return action;

    }

}
