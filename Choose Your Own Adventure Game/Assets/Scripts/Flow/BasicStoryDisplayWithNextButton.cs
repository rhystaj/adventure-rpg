using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

/**
* A basic story display with a button that is hidden when options are displayed.
*/
public class BasicStoryDisplayWithNextButton : BasicStoryDisplay
{

    private Button nextButton; //The button used to progress the story, to be hidden when options are present.

    //Assertion fields.
    private Button nextButtonOnConstruction;

    /**
     * Constructor
     */ 
    public BasicStoryDisplayWithNextButton(string storyText, Canvas canvas, Text displayText, Button nextButton, DynamicButtonGroup optionsGroup) : 
        base(storyText, canvas, displayText, optionsGroup) {

        //Preconditions
        Assert.IsNotNull(nextButton, "Precondition Fail: The argument 'nextButton' should not be null.");


        this.nextButton = nextButton;
        this.nextButton.gameObject.SetActive(true);


        //Assertion only setup.
        Assert.IsTrue(RecordVariables());


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsNotNull(this.nextButton, "Postcondition Fail: 'nextButton' should not be null.");
        Assert.IsTrue(nextButton.IsActive(), "Postcondition Fail: 'nextButton' should be active.");

    }

    public override bool Next()
    {

        bool result = false;

        if (base.Next())
        {
            //Hide the next button if there are options to display.
            nextButton.gameObject.SetActive(false);
            result = true;
        }


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

        return result;
    }

    protected override void OnSelectOption(int optionNumber)
    {

        //Preconditions
        Assert.IsTrue(optionNumber >= 0, "Precondition Fail: 'optionNumber' should be positive.");


        base.OnSelectOption(optionNumber);
        if (nextButton != null) nextButton.gameObject.SetActive(true);


    }


    //Assertion methods.
    private bool RecordVariables()
    {
        nextButtonOnConstruction = nextButton;
        return true;
    }

    protected new bool ClassInvariantsHold()
    {
        
        Assert.IsTrue(nextButton == nextButtonOnConstruction,
                      "Postcondition Fail: The object referenced by 'nextButton' should not be changed at runtime.");
        return true;
    }

}
