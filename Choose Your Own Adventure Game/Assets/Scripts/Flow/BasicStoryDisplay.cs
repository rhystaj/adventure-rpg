using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class BasicStoryDisplay : StoryDisplay
{
    private Text displayText; //The text object where the story will be displayed.
    private Transform optionsOrigin; //The transform used to determine the placement of the buttons.
    private Button buttonBase; //The base button to be cloned.
    private Vector2 buttonRelativeDifferences; //The x and y distances between buttons.

    private string textBeingDisplayed = ""; //The story text being displayed on the screen at any given point. 
    private List<Button> optionButtons; //All buttons representing an option.

    /**
     * Constructor
     */ 
    public BasicStoryDisplay(string storyText, Canvas canvas, Text displayText, Transform optionsOrigin,
        Button buttonBase, Vector2 buttonRelativeDifferences) : 
        base(storyText, canvas) {

        this.displayText = displayText;
        this.optionsOrigin = optionsOrigin;
        this.buttonBase = buttonBase;
        this.buttonRelativeDifferences = buttonRelativeDifferences;

        optionButtons = new List<Button>();

        base.Next();

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

        //Add the new text to the text on screen.
        textBeingDisplayed = textBeingDisplayed.Insert(textBeingDisplayed.Length, text);
        displayText.text = textBeingDisplayed;

    }

    protected override void DisplayOption(int optionNumber, int totalOptions, string text, Canvas canvas, StoryDisplay parser)
    {

        //Create the new button from the prefab and configure position in regards to parent (button origin).
        Button newButton = UnityEngine.Object.Instantiate(buttonBase, Vector3.zero, Quaternion.identity, optionsOrigin);
        newButton.transform.localPosition = new Vector3(
            optionNumber * buttonRelativeDifferences.x - buttonRelativeDifferences.x * totalOptions / 2,
            optionNumber * buttonRelativeDifferences.y - buttonRelativeDifferences.y * totalOptions / 2,
            newButton.transform.localPosition.z //Don't change z.
        );


        //Configure button as an option button.
        newButton.GetComponentInChildren<Text>().text = text;
        newButton.onClick.AddListener(delegate () {
            Debug.Log("Selecting Option " + optionNumber);
            OnSelectOption(optionNumber);
        });
        newButton.name = text;
        optionButtons.Add(newButton);

    }

    /**
     * The method that is called when an option is selected.
     */ 
    protected virtual void OnSelectOption(int optionNumber)
    {

        Debug.Log("Option " + optionNumber + " Selected.");

        //Clear display to show next part of the story.
        textBeingDisplayed = "";
        displayText.text = textBeingDisplayed;

        Debug.Log("optionButtons.Count = " + optionButtons.Count);

        foreach (Button b in optionButtons)
        { //Remove all current buttons on screen.
            Debug.Log(b);
            UnityEngine.Object.DestroyImmediate(b.gameObject);
        }

        optionButtons.Clear();


        //Select the option in the story parser.
        FollowPath(optionNumber);

    }

    protected override void OnStoryEnd() { }
}
