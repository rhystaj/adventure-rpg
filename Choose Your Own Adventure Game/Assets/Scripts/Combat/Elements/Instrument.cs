using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A unit's main item they use in combat.
 */ 
public abstract class Instrument : ScriptableObject {

    /**
     * Attempt to perform the relevant action on the given target 
     */ 
    public abstract bool Use(Unit.IInstance user, Unit.IInstance target);

    /**
     * Returns whether the given user can use the instrument on the given target.
     */ 
    public abstract bool CanUse(Unit.IInstance user, Unit.IInstance target); 

}
