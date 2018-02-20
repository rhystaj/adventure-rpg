using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BarStatDisplay : StatDisplay
{
    public override void VisualiseStat(float value, float max)
    {

        //Preconditions
        Assert.IsTrue(value < max, "Precondition Fail: The given value should be less than the given max.");
        Assert.IsNotNull(GetComponent<Image>(), "Precondition Fail: This component's game object should have an Image component attatched.");


        GetComponent<Image>().fillAmount = value / max;

    }

}
