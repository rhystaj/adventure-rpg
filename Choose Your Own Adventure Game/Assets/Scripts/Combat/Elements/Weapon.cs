using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An instrument that damages an enemy unit.
 */ 
public class Weapon : ScriptableObject, IInstrument {

    public int damage;
    public int minRange; //The closest tile a weapon can hit.
    public int maxRange; //The furthest tile a weapon can hit. 

    public bool Use(Unit.IInstance user, Unit.IInstance target)
    {

        if (user.alignment == target.alignment) return false; //    A unit can not use a weapon on someone from thier own team.
        if (target.position < minRange || target.position > maxRange) return false; //  A unit can not use a weapon on someone out of range. 

        target.health -= damage;

        return true;

    }
}
