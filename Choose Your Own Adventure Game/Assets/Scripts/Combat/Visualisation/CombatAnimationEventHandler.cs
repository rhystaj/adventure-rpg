using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationEventHandler : MonoBehaviour {

	public virtual void OnAttackConnect()
    {

        Debug.Log("On Attack Connect");

    }

    public virtual void OnMoveEnd()
    {
        Debug.Log("On Move End");
    }

    public void Test()
    {
        Debug.Log("Test");
    }

}
