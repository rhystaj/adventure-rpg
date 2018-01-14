using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class BasicStoryInit : MonoBehaviour {

    [SerializeField] protected TextAsset storyFile; //The file the story in being loaded from.
    [SerializeField] protected Text displayText; //The text object where the story will be displayed.
    [SerializeField] protected Button nextButton; //The button used to progress the story.
    [SerializeField] protected DynamicButtonGroup optionsGroup;

    protected BasicStoryDisplay display;

    //Assertion fields
    private string storyTextOnAwake;
    private Text displayTextOnAwake;
    private Button nextButtonOnAwake;
    private DynamicButtonGroup optionsGroupOnAwake;
    private BasicStoryDisplay displayOnAwake;

    protected virtual void Awake()
    {

        //Preconditions
        Assert.IsNotNull(storyFile, "Precondition Fail: 'storyFile' should not be null.");
        Assert.IsNotNull(displayText, "Precondition Fail: 'display' should not be null.");
        Assert.IsNotNull(nextButton, "Precondition Fail: 'nextButton' should not be null.");
        Assert.IsNotNull(optionsGroup, "Precondition Fail: 'optionsGroup' should not be null");


        //Assertion only setup
        Assert.IsTrue(RecordVaraibles());


        display = new BasicStoryDisplayWithNextButton(storyFile.text, null, displayText, nextButton, optionsGroup);


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsNotNull(display, "Postcondition Fail: The field 'display' should not be null.");

    }

    /**
     * Progresses the story when there are no options avaliable.
     */
    public void ContinueStory()
    {

        //Preconditions
        Assert.IsNotNull(display, "Precondition Fail: The field 'display' is not null");


        display.Next();


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

    }


    //Assertion methods
    protected virtual bool RecordVaraibles()
    {

        storyTextOnAwake = storyFile.text;
        displayTextOnAwake = displayText;
        nextButtonOnAwake = nextButton;
        optionsGroupOnAwake = optionsGroup;

        return true;

    }

    protected virtual bool ClassInvariantsHold()
    {

        Assert.AreEqual(storyFile.text, storyTextOnAwake, "Postcondition Fail: The text in story file should not change at runtime.");
        Assert.IsTrue(displayText == displayTextOnAwake, "Postcondition Fail: The object referenced by 'displayText' should not change at runtime.");
        Assert.IsTrue(nextButton == nextButtonOnAwake, "Postcondition Fail: The object referenced by 'nextButton' should not change at runtime.");
        Assert.IsTrue(optionsGroup == optionsGroupOnAwake, "Postcondition Fail: The object referenced by 'optionsGroup' should not change at runtime.");

        return true;

    }

}
