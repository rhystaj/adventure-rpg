using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class BasicStoryDisplay : StoryDisplay
{
    private Text displayText; //The text object where the story will be displayed.
    private DynamicButtonGroup optionsDisplay; //The button group that will display the options.

    private string textBeingDisplayed = ""; //The story text being displayed on the screen at any given point. 

    /**
     * Constructor
     */ 
    public BasicStoryDisplay(string storyText, Canvas canvas, Text displayText, DynamicButtonGroup optionsDisplay) : 
        base(storyText, canvas) {

        this.displayText = displayText;
        this.optionsDisplay = optionsDisplay;

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
        Debug.Log(optionsDisplay);
        optionsDisplay.AddOption(optionNumber, text, () => { OnSelectOption(optionNumber); });
        optionsDisplay.DrawGroup();
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

        //Select the option in the story parser.
        FollowPath(optionNumber);

        optionsDisplay.Clear();

    }

    protected override void OnStoryEnd() { }
}
