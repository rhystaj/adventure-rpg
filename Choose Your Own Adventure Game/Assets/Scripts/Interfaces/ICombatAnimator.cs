using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatAnimator {

    /**
     * Puts the vessel of the given unit in the pose associated with the given state, for the given length of time.
     */ 
    IEnumerator PoseUnit(Unit.IInstance unit, Unit.State poseState);

    /**
     * Updates the display of the unit's stats.
     */ 
    IEnumerator UpdateUnitOverlay(Unit.IInstance unit);

}
