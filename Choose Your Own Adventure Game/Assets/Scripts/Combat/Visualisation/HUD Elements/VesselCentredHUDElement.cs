using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * A CombatHUD element thats appearance is based on that of a UnitVessel.
 */
public class VesselCentredHUDElement : MonoBehaviour {

    [SerializeField] Vector2 positionFromUnit;

    public virtual void ReconfigureAppearance(Camera viewingCamera, CombatBoard.UnitVessel targetVessel)
    {

        //Preconditions
        Assert.IsNotNull(viewingCamera, "Precondition Fail: The argument 'viewingCamera' should not be null.");
        Assert.IsNotNull(viewingCamera, "Precondition Fail: The argument 'targetVessel' should not be null.");


        transform.position = viewingCamera.WorldToScreenPoint(targetVessel.transform.position) +
                             new Vector3(positionFromUnit.x, positionFromUnit.y, transform.position.z);

    }

}
