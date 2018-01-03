using UnityEngine;
using UnityEngine.Assertions;
using Ink.Runtime;

/**
 * Keeps track of progession through a story coded in Ink (exported to JSON) and displays it on a HUD.
 */ 
public abstract class StoryDisplay {

    private Story story; //The Ink story being parsed from the text.
    private Canvas canvas; //The GUI the story will be drawn to.

    private bool lastTextDrawn; //Whether or not the last text of the branch has been drawn.
    private bool atScriptEnd; //Whether or not we are at the end of the script, i.e there are no more text or choices.

    //Assertion Varaibles
    private Story storyOnConstruction;
    private Canvas canvasOnConstruction;

    /**
     * Constructor
     * Initialise fields and display start of story.
     */ 
    public StoryDisplay(string storyText, Canvas canvas)
    {

        //Preconditions
        Assert.IsNotNull(storyText, "Precondition Fail: The 'storyText' argument should not be null.");
        Assert.IsNotNull(canvas, "Precondition Fail: The canvas argument should not be null.");


        //Assign field values.
        story = new Story(storyText);
        this.canvas = canvas;

 
        //Preform any pre-configurations needed.
        ConfigureStory(story);


        //Assertion only setup methods.
        Assert.IsTrue(RecordFinalVariables());


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsNotNull(story, "Postcondition Fail: The 'story' field should not be null.");
        Assert.IsNotNull(canvas, "Postcondition Fail: The canvas field should not be null.");

    }

    /**
     * If there is still text to parse in this branch, draw it to the screen and return true if the most recent text to be  
     * drawn is the last text in the branch.
     */ 
    public bool Next()
    {

        bool result;

        if (lastTextDrawn) //If the last text in the branch has already been drawn.
        {

            Assert.IsFalse(story.canContinue, "If the last text is drawn, the story should have no more text in is branch.");

            if (atScriptEnd)
            {

                Assert.AreEqual(story.currentChoices.Count, 0, "If the story has ended, there should be no more choices avaliable.");

                OnStoryEnd();

            }

            lastTextDrawn = false;
            result = true;
        }
        else
        {

            Assert.IsTrue(story.canContinue, "The story should still have some text on its current branch.");

            //Get next line of text and draw it to the screen.
            string next = story.Continue();
            DisplayMoment(next, canvas, this);

            lastTextDrawn = !story.canContinue;
            atScriptEnd = lastTextDrawn && story.currentChoices.Count == 0;
            result = false;
        }


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

        return result;

    }

    /**
     * Select the branch in the story to go down.
     */ 
    protected void FollowPath(int pathNum)
    {

        //Preconditions
        Assert.IsTrue(story.currentChoices.Count > 0, "Precondition Fail: There should be options avaliable to be taken.");
        Assert.IsFalse(story.canContinue, "Precondition Fail: There should be no more text to display in the current branch.");
        Assert.IsTrue(pathNum >= 0 && pathNum < story.currentChoices.Count,
                      "Precondition Fail: pathNum should be a valid choice number.");


        story.ChooseChoiceIndex(pathNum);
        Next();


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsTrue(story.canContinue || story.currentChoices.Count > 0,
                      "Postcondition Fail: The story should either have text or options avaliable.");

    }

    /**
     * Draw the options to the screen and returns true, if there are any options.
     */ 
    protected bool ShowOptions()
    {

        bool result;

        if (story.currentChoices.Count <= 0) result = false; //There are no choices, so don't bother drawing them.
        else
        {
            //Draw every option to the screen.
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                DisplayOption(i, story.currentChoices.Count, story.currentChoices[i].text, canvas, this);
            }


            result = true; //Options were drawn.
        }


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

        return result;
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



    //Assertion methods.
    private bool RecordFinalVariables()
    {
        storyOnConstruction = story;
        canvasOnConstruction = canvas;
        return true;
    }

    private bool ClassInvariantsHold()
    {
        Assert.IsTrue(story == storyOnConstruction, "Postcondition Fail: The object referenced by 'story' should not change at runtime.");
        Assert.IsTrue(canvas == canvasOnConstruction, "Postcondition Fail: The object referenced by 'canvas' should not change at runtime.");
        return true;
    }

}
