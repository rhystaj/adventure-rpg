using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicStoryInit : MonoBehaviour {

    [SerializeField] protected TextAsset storyFile; //The file the story in being loaded from.
    [SerializeField] protected Text displayText; //The text object where the story will be displayed.
    [SerializeField] protected Button nextButton; //The button used to progress the story.
    [SerializeField] protected DynamicButtonGroup optionsGroup;

    protected BasicStoryDisplay display;

    protected virtual void Awake()
    {
        display = new BasicStoryDisplayWithNextButton(storyFile.text, null, displayText, nextButton, optionsGroup);
    }

    /**
     * Progresses the story when there are no options avaliable.
     */
    public void ContinueStory()
    {
        Debug.Log("Next");
        display.Next();
    }

}
