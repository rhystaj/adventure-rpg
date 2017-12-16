using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButtonGroup : MonoBehaviour {

    private enum VerticalAlignment { top = 0, centre = 1, bottom = 2 };
    private enum HorizontalAlignment { left = 0, centre = 0, right = 2};

    [SerializeField] Button buttonBase; //The base button to be cloned.
    [SerializeField] Vector2 buttonRelativeDifferences; //The x and y distances between buttons.
    [SerializeField] VerticalAlignment verticalAlignment;
    [SerializeField] HorizontalAlignment horizontalAlignment;

    private int numberOfOptions; //The total number of options added to the group.
    private List<Button> buttons = new List<Button>(); //The buttons that have been added to the group so far.

    /**
     * Add a button with the given option and appropriate behaviour.
     */ 
    public void AddOption(int optionNumber, string optionText, Action OnSelected)
    {
        //Ensure the list is at least as big as the given option number.
        while (buttons.Count <= optionNumber) buttons.Add(null);

        Debug.Log(buttons.Count);

        //Make a copy of the button, with the given text and option, but do not yet instantiate it.
        buttons[optionNumber] = Instantiate(buttonBase, transform);
        buttons[optionNumber].gameObject.SetActive(false);

        //Configure button as an option button.
        buttons[optionNumber].GetComponentInChildren<Text>().text = optionText;
        buttons[optionNumber].onClick.AddListener(delegate () {
            OnSelected();
        });
        buttons[optionNumber].name = optionText;

        numberOfOptions++;

    }

    /**
     * Draw the button group to the screen.
     */ 
    public void DrawGroup()
    {
        for (int i = 0; i < numberOfOptions; i++)
        {

            //Calculate horizontal and vertical offsets based on the given alignment values.
            float horizontalOffset = buttonRelativeDifferences.x * numberOfOptions / 2 * (float)horizontalAlignment;
            float verticalOffset = buttonRelativeDifferences.y * numberOfOptions / 2 * (float)verticalAlignment;

            if (buttons[i] == null) throw new Exception("There is no button in position " + i + ".");

            //Create the new button from the prefab and configure position in regards to parent (button origin).
            buttons[i].transform.localPosition = new Vector3(
                i * buttonRelativeDifferences.x - horizontalOffset,
                i * buttonRelativeDifferences.y - verticalOffset,
                buttons[i].transform.localPosition.z //Don't change z.
            );

            buttons[i].gameObject.SetActive(true);

        }
    }

    /**
     * Remove all buttons from the group.
     */ 
    public void Clear()
    {
        //Remove all buttons from the screen.
        foreach (Button button in buttons)
            DestroyImmediate(button.gameObject);

        buttons.Clear();

        numberOfOptions = 0;
    }

}
