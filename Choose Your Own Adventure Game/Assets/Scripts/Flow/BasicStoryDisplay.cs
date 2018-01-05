using Ink.Runtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class BasicStoryDisplay : StoryDisplay
{
    private Text displayText; //The text object where the story will be displayed.
    private DynamicButtonGroup optionsDisplay; //The button group that will display the options.
    

    private string textBeingDisplayed = ""; //The story text being displayed on the screen at any given point.

    //Assertion Fields
    public Text displayTextOnConstruction;
    public DynamicButtonGroup optionsDisplayOnConstruction;

    /**
     * Constructor
     */ 
    public BasicStoryDisplay(string storyText, Canvas canvas, Text displayText, DynamicButtonGroup optionsDisplay) : 
        base(storyText, canvas) {

        //Preconditions
        Assert.IsNotNull(displayText, "Precondition Fail: The 'displayText' argument should not be null.");
        Assert.IsNotNull(optionsDisplay, "PreconditionFail: The 'optionsDisplay' argument should not be null.");

        
        //Set field values.
        this.displayText = displayText;
        this.optionsDisplay = optionsDisplay;

        base.Next();


        //Assertion only setup.
        Assert.IsTrue(RecordValues());
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsNotNull(this.displayText, "Postcondition Fail: The field 'displayText' should not be null.");
        Assert.IsNotNull(this.optionsDisplay, "Postcondition Fail: The field 'optionsDisplay should not be null'");

    }
    
    /**
     * Displays the next part of the text.
     */ 
    public new virtual bool Next()
    {

        if (base.Next())
        {
            //The parser has reached the end of the current branch, so show the current options.
            ShowOptions();
            return true;
        }
        else return false;

    }

    protected override void ConfigureStory(Story story) {   }

    protected override void DisplayMoment(string text, Canvas canvas, StoryDisplay parser)
    {

        //Preconditions
        Assert.IsNotNull(text, "Precondition Fail: The argument 'text' should not be null.");
        Assert.IsNotNull(canvas, "Precondition Fail: The argument 'canvas' should not be null.");
        Assert.IsNotNull(parser, "Precondition Fail: The argument 'parser' should not be null.");


        string oldText = textBeingDisplayed; //Record text before use for assertions.


        //Add the new text to the text on screen.
        textBeingDisplayed = textBeingDisplayed.Insert(textBeingDisplayed.Length, text);
        displayText.text = textBeingDisplayed;


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());
        Assert.AreEqual(textBeingDisplayed, oldText + text,
                        "Postcondition Fail: The new text should be the previous text with the 'text argument' appended to the end.");
        Assert.AreEqual(displayText.text, textBeingDisplayed,
                        "Postcondition Fail: 'textBeingDisplayed' should be being displayed to on the screen.");


    }

    protected override void DisplayOption(int optionNumber, int totalOptions, string text, Canvas canvas, StoryDisplay parser)
    {

        //Preconditions
        Assert.IsTrue(optionNumber >= 0 && optionNumber < totalOptions,
                      "Precondition Fail: The argument 'optionNumber' should be between 0 and the total number of options.");
        Assert.IsNotNull(text, "Precondition Fail: The argument 'text' should not be null.");
        Assert.IsNotNull(canvas, "Precondition Fail: The argument 'canvas' should not be null");
        Assert.IsNotNull(parser, "Precondition Fail: The argument 'parser' should not be null");
        Assert.IsNotNull(optionsDisplay, "Precondition Fail: The argument 'optionsDisplay' should not be null");


        optionsDisplay.AddOption(optionNumber, text, () => { OnSelectOption(optionNumber); });
        optionsDisplay.DrawGroup();


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

    }

    /**
     * The method that is called when an option is selected.
     */ 
    protected virtual void OnSelectOption(int optionNumber)
    {

        //Preconditions
        Assert.IsTrue(optionNumber >= 0, "Precondition Fail: The argument 'optionNumber' should be greater than 0.");
        Assert.IsNotNull(displayText, "Precondition Fail: 'displayText' should not be null.");

        //Clear display to show next part of the story.
        textBeingDisplayed = "";
        displayText.text = textBeingDisplayed;


        //Select the option in the story parser.
        FollowPath(optionNumber);


        optionsDisplay.Clear();


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

    }

    protected override void OnStoryEnd() { }


    //Assersion methods.
    private bool RecordValues()
    {
        displayTextOnConstruction = displayText;
        optionsDisplayOnConstruction = optionsDisplay;
        return true;
    }

    protected override bool ClassInvariantsHold()
    {
        base.ClassInvariantsHold();
        Assert.IsTrue(displayText == displayTextOnConstruction,
                      "Postcondition Fail: The object referenced by 'displayText' should not be changed at runtime.");
        Assert.IsTrue(optionsDisplay == optionsDisplayOnConstruction,
                      "Postcondition Fail: The object referenced by 'optionsDisplay' should not be changed at runtime.");
        return true;
    }
}
