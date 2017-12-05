using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenarioManager : BasicStoryInit {

    [SerializeField] protected NavigationManager navigationManager; // The object handling player navigation.
    [SerializeField] private Canvas displayCanvas; // The canvas where the story will be displayed.

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        MapNode.NodeReached += OnReachNewNode;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        MapNode.NodeReached -= OnReachNewNode;
    }

    protected override void Awake() { /*Initialisation will be handled in OnSceneLoaded instead.*/ }

    /**
     * What happens when the scene is first loaded.
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ShowScenarioText(storyFile.text, navigationManager.startingLocation);
    }

    /**
     * The actions performed when the player arrives at a new node.
     */ 
    private void OnReachNewNode(MapNode node)
    {
        ShowScenarioText(((Scenario)display).GetScriptForNode(node), node);
    }

    private void ShowScenarioText(string text, MapNode location)
    {
        displayCanvas.gameObject.SetActive(true);
        navigationManager.enabled = false;

        display = new Scenario(text, displayCanvas, location, displayText, nextButton, optionsOrigin,
                                       buttonBase, buttonRelativeDifferences, OnScenarioEnd);
    }

    /**
     * The action to be performed when the current scenario has ended, i.e. reached the end of the ink script.
     */ 
    private void OnScenarioEnd()
    {
        Debug.Log("Scenario Ended.");

        displayCanvas.gameObject.SetActive(false);
        navigationManager.enabled = true;
    }

    /**
     * Reprents a single story scenario.
     */ 
    private class Scenario : BasicStoryDisplayWithNextButton
    { 

        private MapNode location; //    The point on the map where this scenario takes place.
        private Action OnScenarioEnd; //     The action to be performed when the scenarion has ended.

        //  The scenarios that play out at each of the avaliable destinations.
        private Dictionary<MapNode, string> scenarioAtLocation = new Dictionary<MapNode, string>();

        public Scenario(string storyText, Canvas canvas, MapNode location, Text displayText, Button nextButton,
                        Transform optionsOrigin, Button buttonBase, Vector2 buttonRelativeDifferences, Action OnScenarioEnd) :
            base(storyText, canvas, displayText, nextButton, optionsOrigin, buttonBase, buttonRelativeDifferences)
        {
            this.location = location;
            this.OnScenarioEnd = OnScenarioEnd;
            location.CloseAllPaths();
        }

        protected override void ConfigureStory(Story story)
        {
            //Bind open story path to the avaliable functions in the story.
            story.BindExternalFunction("openStoryPath", (string destinationNodeName, string scenarioScriptPath) =>
            {
                OpenStoryPath(destinationNodeName, scenarioScriptPath);
            }
            );

            //Bind open story path to the avaliable functions in the story.
            story.BindExternalFunction("endGame", () =>
            {
                Application.Quit();
            }
            );
        }

        /**
         * Make the path with the given name avaliable from the current location, and set its associated scenario.
         */
        private void OpenStoryPath(string destinationNodeName, string scenarioScriptPath)
        {

            //Retrieve the given node and text file, and throw errors if they don't exist.
            MapNode destination = location.GetNeighbourWithName(destinationNodeName);
            if (destination == null)
                throw new System.Exception("The map node " + location.name + " does not have a neighbour named " + destinationNodeName + ".");
            TextAsset script = Resources.Load<TextAsset>(scenarioScriptPath);
            if (script == null)
                throw new System.Exception("The resource " + scenarioScriptPath + ".json does not exist.");


            //  Map the location to its scenario and make the respctive path traversable.
            location.GetPathTo(destination).traversable = true;
            scenarioAtLocation.Add(destination, script.text);

        }

        /**
         * Returns the script for the scenario of the given neighboring node.
         */ 
        public string GetScriptForNode(MapNode node)
        {
            return scenarioAtLocation[node];
        }

        protected override void OnStoryEnd()
        {
            OnScenarioEnd();
        }
    }

}
