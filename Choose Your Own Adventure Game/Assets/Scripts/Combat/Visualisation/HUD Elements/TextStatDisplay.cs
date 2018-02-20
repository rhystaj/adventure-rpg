using UnityEngine.Assertions;
using UnityEngine.UI;

public class TextStatDisplay : StatDisplay
{

    public override void VisualiseStat(float value, float max)
    {


        //Preconditions
        Assert.IsTrue(value <= max, "Precondition Fail: The given value should be less than or equal to the given max.");
        Assert.IsNotNull(GetComponent<Text>(), "Precondition Fail: This component's game object should have a Text component attatched.");


        GetComponent<Text>().text = value + "";

    }

}
