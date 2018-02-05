﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A unit's main item they use in combat.
 */ 
public interface IInstrument {

    /**
     * Attempt to perform the relevant action on the given target 
     */ 
    bool Use(Unit.IInstance user, Unit.IInstance target);

    /**
     * Returns whether the given user can use the instrument on the given target.
     */ 
    bool CanUse(Unit.IInstance user, Unit.IInstance target); 

}
