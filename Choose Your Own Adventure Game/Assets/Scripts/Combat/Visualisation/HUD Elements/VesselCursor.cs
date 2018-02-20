using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VesselCursor : VesselCentredHUDElement {

	private CombatBoard.UnitVessel _vesselCurretlyHighlighting; //The current vessel the cursor is being used to highlight.
    public CombatBoard.UnitVessel vesselCurrentlyHighlighting { get { return _vesselCurretlyHighlighting; } }

    public override void ReconfigureAppearance(Camera viewingCamera, CombatBoard.UnitVessel targetVessel)
    {

        Debug.Log("Vessel Cursor");

        gameObject.SetActive(true);
        _vesselCurretlyHighlighting = targetVessel;


        base.ReconfigureAppearance(viewingCamera, targetVessel);

    }

    public void HighlightNothing()
    {

        transform.position = Vector3.zero;
        gameObject.SetActive(false);
        _vesselCurretlyHighlighting = null;

    }

}
