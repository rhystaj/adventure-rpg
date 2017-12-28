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
    public abstract bool Use(Unit user, Unit target);

}
