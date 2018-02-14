using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CombatAnimator : ICombatAnimator
{

    private Dictionary<Unit.IInstance, CombatBoard.UnitVessel> vessels = new Dictionary<Unit.IInstance, CombatBoard.UnitVessel>(); //The instances of units mapped to the vessels that house them.

    public CombatAnimator(CombatBoard.UnitVessel[] vessels)
    {

        //Preconditions
        Assert.IsNotNull(vessels, "The argument 'vessels' should not be null.");


        foreach (CombatBoard.UnitVessel vessel in vessels)
            this.vessels.Add(vessel.unit, vessel);


    }


    public IEnumerator PoseUnit(Unit.IInstance unit, Unit.State poseState)
    {

        //Preconditions
        Assert.IsNotNull(unit, "Precondition Fail: The argument 'unit' should not be null.");
        Assert.IsTrue(vessels.ContainsKey(unit),
                      "Precondition Fail: The given unit should be a key in state.");

        Debug.Log("Posing: " + unit);

        vessels[unit].SetPose(poseState);
        yield return null;

    }

}
