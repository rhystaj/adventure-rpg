using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

/**
 * Keeps track of progession through a story coded in Ink (exported to JSON) and displays it on a HUD.
 */ 
public abstract class StoryDisplay : ScriptParser{

    private Story story; //The Ink story being parsed from the text.
    private Canvas canvas; //The GUI the story will be drawn to.

    private bool lastTextDrawn; //Whether or not the last text of the branch has been drawn.
    private bool atScriptEnd; //Whether or not we are at the end of the script, i.e there are no more text or choices.

    /**
     * Constructor
     * Initialise fields and display start of story.
     */ 
    public StoryDisplay(string storyText, Canvas canvas)
    {

        //Assign field values.
        story = new Story(storyText);
        this.canvas = canvas;


        //Preform any pre-configurations needed.
        ConfigureStory(story);

    }

    /**
     * If there is still text to parse in this branch, draw it to the screen and return true if the most recent text to be  
     * drawn is the last text in the branch.
     */ 
    public bool Next()
    {

        if (lastTextDrawn) //If the last text in the branch has already been drawn.
        {
            if (atScriptEnd) OnStoryEnd();
            lastTextDrawn = false;
            return true;
        } 


        //Get next line of text and draw it to the screen.
        string next = story.Continue();
        DisplayMoment(next, canvas, this);

        lastTextDrawn = !story.canContinue;
        atScriptEnd = lastTextDrawn && story.currentChoices.Count == 0;
        return false;

    }

    /**
     * Select the branch in the story to go down.
     */ 
    protected void FollowPath(int pathNum)
    {
        story.ChooseChoiceIndex(pathNum);
        Next();
    }

    /**
     * Draw the options to the screen and returns true, if there are any options.
     */ 
    protected bool ShowOptions()
    {

        if (story.currentChoices.Count <= 0) return false; //There are no choices, so don't bother drawing them.


        //Draw every option to the screen.
        for(int i = 0; i < story.currentChoices.Count; i++)
        {
            DisplayOption(i, story.currentChoices.Count , story.currentChoices[i].text, canvas, this);
        }


        return true; //Options were drawn.
    }

    /**
     * Perform any actions needed before the story begins to be parsed.
     */ 
    protected abstract void ConfigureStory(Story story);

    /**
     * Draws to the GUI based on the text in the story..
     */ 
    protected abstract void DisplayMoment(string text, Canvas canvas, StoryDisplay parser);

    /**
     * Draws the numbered option to the screen.
     */
    protected abstract void DisplayOption(int optionNumber, int totalOptions, string text, Canvas canvas, StoryDisplay parser);

    /**
     * Called when the end of the file has been reached.
     */ 
    protected abstract void OnStoryEnd();
}
