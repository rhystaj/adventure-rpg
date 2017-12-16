using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
* A basic story display with a button that is hidden when options are displayed.
*/
public class BasicStoryDisplayWithNextButton : BasicStoryDisplay
{

    private Button nextButton; //The button used to progress the story, to be hidden when options are present.

    /**
     * Constructor
     */ 
    public BasicStoryDisplayWithNextButton(string storyText, Canvas canvas, Text displayText, Button nextButton, DynamicButtonGroup optionsGroup) : 
        base(storyText, canvas, displayText, optionsGroup) {

        this.nextButton = nextButton;
        this.nextButton.gameObject.SetActive(true);

    }

    public override bool Next()
    {

        if (base.Next())
        {
            //Hide the next button if there are options to display.
            nextButton.gameObject.SetActive(false);
            return true;
        }
        else return false;

    }

    protected override void OnSelectOption(int optionNumber)
    {
        Debug.Log("Showing Button");

        base.OnSelectOption(optionNumber);
        if (nextButton != null) nextButton.gameObject.SetActive(true);
    }
    
}
