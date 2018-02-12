using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An instrument that damages an enemy unit.
 */
[CreateAssetMenu]
public class Weapon : Instrument {

    public int damage;
    public int minRange; //The closest tile a weapon can hit.
    public int maxRange; //The furthest tile a weapon can hit. 

    public override bool CanUse(Unit.IInstance user, Unit.IInstance target)
    {
        if (user.alignment == target.alignment) return false; //    A unit can not use a weapon on someone from thier own team.
        if (target.position < minRange || target.position > maxRange) return false; //  A unit can not use a weapon on someone out of range. 

        return true;

    }

    public override bool Use(Unit.IInstance user, Unit.IInstance target)
    {

        if (!CanUse(user, target)) return false;

        target.health -= damage;

        return true;

    }

}
