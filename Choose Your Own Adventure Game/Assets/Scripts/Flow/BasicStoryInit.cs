using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicStoryInit : MonoBehaviour {

    [SerializeField] protected TextAsset storyFile; //The file the story in being loaded from.
    [SerializeField] protected Text displayText; //The text object where the story will be displayed.
    [SerializeField] protected Button nextButton; //The button used to progress the story.
    [SerializeField] protected Transform optionsOrigin; //The transform used to determine the placement of the buttons.
    [SerializeField] protected Button buttonBase; //The base button to be cloned.
    [SerializeField] protected Vector2 buttonRelativeDifferences; //The x and y distances between buttons.

    protected BasicStoryDisplay display;

    protected virtual void Awake()
    {
        display = new BasicStoryDisplayWithNextButton(storyFile.text, null, displayText, nextButton, optionsOrigin, buttonBase, buttonRelativeDifferences);
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
