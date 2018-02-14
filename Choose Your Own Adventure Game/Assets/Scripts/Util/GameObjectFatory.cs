using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/**
 * Programatically creates game objects with the appropriate components.
 */ 
public class GameObjectFatory {

    /**
     * Create a new game object with the given name, a text component, and all other neccessary components.
     * Set its parent to the given transform and give the text component initialText.
     * Return the text component.
     */ 
	public static Text CreateText(string name, Transform parent, string initialText)
    {

        //Precondition
        Assert.IsNotNull(name, "Precondition Fail: The argument 'name' should not be null.");
        Assert.IsNotNull(parent, "Precondition Fail: The argument 'parent' should not be null.");
        Assert.IsNotNull(initialText, "Precondition Fail: The argument 'initialText' should not be null.");


        GameObject newObject = new GameObject(name, new Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Text) });

        newObject.transform.SetParent(parent);
        newObject.GetComponent<Text>().text = initialText;

        Text result = newObject.GetComponent<Text>();


        //Postconditions
        Assert.IsNotNull(result, "Postcondition Fail: The result should not be null.");
        Assert.IsNotNull(result.GetComponent<RectTransform>(), "Postcondition Fail: The result should have a RectTransform attatched.");
        Assert.IsNotNull(result.GetComponent<CanvasRenderer>(), "Postcondition Fail: The result should have a canvas renderer attatched.");
        Assert.IsTrue(result.gameObject.name.Equals(name), "Postcondition Fail: The gameObject of the result should have the given name.");
        Assert.IsTrue(result.transform.parent == parent, "Postcondition Fail: The result should have the given parent transform.");
        Assert.IsTrue(result.text.Equals(initialText), "Postcondition Fail: The result should have the given initial text.");

        return result;
    }

}
