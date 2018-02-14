﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CombatAnimator : ICombatAnimator
{

    private Once<CombatHUD> overlay = new Once<CombatHUD>();

    private Dictionary<Unit.IInstance, CombatBoard.UnitVessel> vessels = new Dictionary<Unit.IInstance, CombatBoard.UnitVessel>(); //The instances of units mapped to the vessels that house them.

    public CombatAnimator(CombatBoard.UnitVessel[] vessels, CombatHUD overlay)
    {

        //Preconditions
        Assert.IsNotNull(vessels, "The argument 'vessels' should not be null.");
        Assert.IsNotNull(overlay, "Precondition Fail: The argument 'overlay' should not be null.");

        this.overlay.Value = overlay;

        foreach (CombatBoard.UnitVessel vessel in vessels)
        {
            this.vessels.Add(vessel.unit, vessel);
            this.overlay.Value.UpdateStatsFor(vessel);
        }


    }

    public IEnumerator UpdateUnitOverlay(Unit.IInstance unit)
    {

        //Preconditions
        Assert.IsNotNull(unit, "Precondition Fail: The argument 'unit' should not be null.");
        Assert.IsTrue(vessels.ContainsKey(unit),
                      "Precondition Fail: The given unit should be a key in state.");

        overlay.Value.UpdateStatsFor(vessels[unit]);

        yield return null;

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
