using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * A component that visualises a stat.
 */
public abstract class StatDisplay : VesselCentredHUDElement {

    public abstract void VisualiseStat(float value, float max);

}
