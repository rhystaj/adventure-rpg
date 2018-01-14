using Ink.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class ScenarioManager : BasicStoryInit {

    [SerializeField] protected NavigationManager navigationManager; // The object handling player navigation.
    [SerializeField] private Canvas displayCanvas; // The canvas where the story will be displayed.


    //Assertion Fields
    private NavigationManager navigationManagerOnSceneLoad;
    private Canvas displayCanvasOnSceneLoad;

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

        //Preconditions
        Assert.IsNotNull(navigationManager, "Precondition Fail: 'navigationManager' should not be null.");
        Assert.IsNotNull(displayCanvas, "Precondition Fail: 'displayCanvas' should not be null");

        
        //Assertion only setup
        Assert.IsTrue(RecordVaraibles());


        ShowScenarioText(storyFile.text, navigationManager.startingLocation);


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

    }

    /**
     * The actions performed when the player arrives at a new node.
     */ 
    private void OnReachNewNode(MapNode node)
    {

        //Preconditions
        Assert.IsNotNull(node, "Precondition Fail: The argument 'node' should not be null.");


        ShowScenarioText(((Scenario)display).GetScriptForNode(node), node);


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

    }

    private void ShowScenarioText(string text, MapNode location)
    {

        //Preconditions
        Assert.IsNotNull(text, "Precondition Fail: The argument 'text' should not be null.");
        Assert.IsNotNull(location, "Precondition Fail: The argument 'location' should not be null.");
        Assert.IsNotNull(displayCanvas, "Precondition Fail: The field 'displayCanvas' should not be null.");
        Assert.IsNotNull(navigationManager, "Precondition Fail: The field 'navigationManager' should not be null.");


        displayCanvas.gameObject.SetActive(true);
        navigationManager.enabled = false;

        display = new Scenario(text, displayCanvas, location, displayText, nextButton, optionsGroup, OnScenarioEnd);


        //Postconditions
        Assert.IsTrue(displayCanvas.gameObject.activeSelf, "Postcondition Fail: 'displayCanvas should be active'.");
        Assert.IsFalse(navigationManager.enabled, 
                       "Postcondition Fail: 'navigationManager' should not be enabled - i.e the player can not move during text.");
        Assert.IsTrue(ClassInvariantsHold());

    }

    /**
     * The action to be performed when the current scenario has ended, i.e. reached the end of the ink script.
     */ 
    private void OnScenarioEnd()
    {

        //Preconditions
        Assert.IsNotNull(displayCanvas, "Precondition Fail: The field 'displayCanvas' should not be null.");
        Assert.IsNotNull(navigationManager, "Precondition Fail: The field 'navigationManager' should not be null.");


        displayCanvas.gameObject.SetActive(false);
        navigationManager.enabled = true;


        //Postconditions
        Assert.IsFalse(displayCanvas.gameObject.activeSelf, "Postcondition Fail: 'displayCanvas should not be active'.");
        Assert.IsTrue(navigationManager.enabled,
                       "Postcondition Fail: 'navigationManager' should be enabled - i.e the player can not move now the text has ended.");
        Assert.IsTrue(ClassInvariantsHold());

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


        //Assertion fields
        private MapNode locationOnConsruction;
        private Action OnScenarioEndOnConstruction;
        private Dictionary<MapNode, string> scenarioAtLocationOnConstruction;

        private HashSet<MapNode> givenDestinations;
        private HashSet<string> givenScripts;

        public Scenario(string storyText, Canvas canvas, MapNode location, Text displayText, Button nextButton,
                        DynamicButtonGroup optionsGroup, Action OnScenarioEnd) :
            base(storyText, canvas, displayText, nextButton,optionsGroup)
        {

            //Preconditions
            Assert.IsNotNull(location, "Precondition Fail: The argument 'location' should not be null.");
            Assert.IsNotNull(OnScenarioEnd, "Precondition Fail: The argument 'OnScenarioEnd' should not be null.");


            this.location = location;
            this.OnScenarioEnd = OnScenarioEnd;
            location.CloseAllPaths();


            //Assertion only setup
            Assert.IsTrue(RecordVariables());
            Assert.IsTrue(InitialiseAssertionSets());


            //Postconditions
            Assert.IsTrue(InnerClassInvariantsHold());
            Assert.IsTrue(this.location == location,
                          "Postcondition Fail: The field 'location' should be set to the value of the argument with the same name.");
            Assert.IsTrue(this.OnScenarioEnd == OnScenarioEnd,
                           "Postcondition Fail: The field 'OnScenarioEnd' should be set to the value of the argument with the same name.");
            Assert.IsFalse(this.location.HasTraversableExit(), "Postcondition Fail: Location should not be able to be moved from at this point.");

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
                throw new Exception("The map node " + location.name + " does not have a neighbour named " + destinationNodeName + ".");
            TextAsset script = Resources.Load<TextAsset>(scenarioScriptPath);
            if (script == null)
                throw new Exception("The resource " + scenarioScriptPath + ".json does not exist.");


            //  Map the location to its scenario and make the respctive path traversable.
            location.GetPathTo(destination).traversable = true;
            scenarioAtLocation.Add(destination, script.text);


            //Record added destinations and scripts
            givenDestinations.Add(destination);
            givenScripts.Add(script.text);


            //Postconditions
            Assert.IsTrue(InnerClassInvariantsHold());
            Assert.IsTrue(location.GetPathTo(destination).traversable,
                          "Postcondition Fail: The path to 'destination' should be traversable.");
            Assert.AreEqual(scenarioAtLocation[destination], script.text,
                          "Postcondition Fail: The text in 'script' should be mapped to destination.");

        }

        /**
         * Returns the script for the scenario of the given neighboring node.
         */ 
        public string GetScriptForNode(MapNode node)
        {

            //Preconditions
            Assert.IsNotNull(node, "Precondition Fail: The argument 'node' should not be null");
            Assert.IsTrue(scenarioAtLocation.ContainsKey(node),
                          "Precondition Fail: The given node should have a script mapped to it.");


            string result = scenarioAtLocation[node];


            //Postconditions
            Assert.IsTrue(InnerClassInvariantsHold());
            Assert.IsNotNull(result, "Postcondition Fail: The returned result should not be null.");
            Assert.IsTrue(scenarioAtLocation.ContainsValue(result),
                          "Postcondition Fail: The returned result should be mapped to a value destination.");

            return result;

        }

        protected override void OnStoryEnd()
        {
            OnScenarioEnd();
        }


        //Assertion Methods
        private bool RecordVariables()
        {

            locationOnConsruction = location;
            OnScenarioEndOnConstruction = OnScenarioEnd;
            scenarioAtLocationOnConstruction = scenarioAtLocation;

            return true;

        }

        private bool InitialiseAssertionSets()
        {

            givenDestinations = new HashSet<MapNode>();
            givenScripts = new HashSet<string>();

            return true;

        }

        private bool InnerClassInvariantsHold()
        {

            //Ensure appropriate fields don't change.
            Assert.IsTrue(location = locationOnConsruction,
                          "Postcondition Fail: The object referenced by 'location'  should not be changed at runtime.");
            Assert.IsTrue(OnScenarioEnd == OnScenarioEndOnConstruction,
                          "Postcondition Fail: The object referenced by 'OnScenarioEnd' should not be changed at runtime.");
            Assert.IsTrue(scenarioAtLocation == scenarioAtLocationOnConstruction,
                          "Postcondition Fail: The object referenced by 'scenarioAtLocation' should not change at runtime.");


            //Check entries
            foreach (MapNode node in scenarioAtLocation.Keys)
                Assert.IsTrue(givenDestinations.Contains(node), 
                              "Postcondition Fail: scenarioAtLocation contains a node that was not given in OpenStoryPath().");
            foreach (string script in scenarioAtLocation.Values)
                Assert.IsTrue(givenScripts.Contains(script),
                              "Postcondition Fail: scenarioAtLocation contains a script that was not given in OpenStoryPath().");


            return true;

        }

    }


    //Assertion methods
    protected override bool RecordVaraibles()
    {

        base.RecordVaraibles();

        navigationManagerOnSceneLoad = navigationManager;
        displayCanvasOnSceneLoad = displayCanvas;

        return true;

    }

    protected override bool ClassInvariantsHold()
    {

        base.ClassInvariantsHold();

        Assert.IsTrue(navigationManager == navigationManagerOnSceneLoad,
                      "Postcondition Fail: The object referenced by 'navigationManager' should not be changed during runtime.");
        Assert.IsTrue(displayCanvas == displayCanvasOnSceneLoad,
                      "Postcondition Fail: The object referneced by 'displayCanvas' should not be changed during runtime.");

        return true;

    }


}
