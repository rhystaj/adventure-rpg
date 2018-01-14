using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MapNode : MonoBehaviour {

    [SerializeField] MovementPath[] pathsFrom; //The paths that can be taken from the node.

    //The paths that can be taken from the node to reach another node.
    private Dictionary<MapNode, MovementPath> pathTo = new Dictionary<MapNode, MovementPath>(); 

    public delegate void NodeInteraction(MapNode node); //  A delegate for when a node is interacted with.
    public static event NodeInteraction NodeSelected; //    An event that is fired when a node is selected.
    public static event NodeInteraction NodeReached; //     An event that is fired when the node is reached by the player.


    //Assertion Fields
    private Dictionary<MapNode, MovementPath> pathToOnStart;

    private void Start()
    {

        //Preconditions
        Assert.IsNotNull(pathsFrom, "Precondition Fail: The field 'pathsFrom' should not be null");


        //Populate the pathsTo dictionary with the nodes at the ends of the paths in pathsFrom, mapped to thier respective paths.
        foreach(MovementPath path in pathsFrom)
        {
            //Get the map node components which should be assigned to nodes at either ends of the path.
            MapNode pathStart = path.StartNode.GetComponent<MapNode>();
            MapNode pathEnd = path.EndNode.GetComponent<MapNode>();


            //Both the start and end nodes should have a map node component - throw an error if this is not the case.
            if (pathStart == null)
                throw new Exception(path.StartNode.name + "should have a MapNode compenet assigned to it, as it is the start of a path.");
            if(pathEnd == null)
                throw new Exception(path.EndNode.name + 
                                    "should have a MapNode component assigned to it, as it is the end of a path.");


            //Add pathStart or pathEnd to pathTo, depending in which one is not this node.
            if (pathStart != this)
                pathTo.Add(pathStart, path);
            else if (pathEnd != this)
                pathTo.Add(pathEnd, path);
            else
                throw new Exception(path.name + "should not be assigned as a path from " + name + ", as it does not start or end with it.");

            Assert.IsTrue((pathTo.ContainsKey(pathStart) || pathTo.ContainsKey(pathEnd)) && !pathTo.ContainsKey(this),
                         "The pathTo field of" + name + "should contain the node at the other end of the path " + path.name + " and not itself");
            Assert.IsTrue(pathTo.ContainsValue(path), "The pathTo field of" + name + "should contain the path " + path.name);

        }



        //Assertion only setup
        Assert.IsTrue(RecordVariables());


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

    }

    /**
     * Make all paths from this node untraversable.
     */
    public void CloseAllPaths()
    {

        foreach (MovementPath path in pathTo.Values) path.traversable = false;


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

    }

    /**
     * Returns true if there is a traversable path leading out from the node.
     */
    public bool HasTraversableExit()
    {

        bool result = false;

        foreach (MovementPath path in pathTo.Values)
        {
            if (path.traversable)
            {
                result = true;
                break;
            }
        }


        //Postconditons
        Assert.IsTrue(ClassInvariantsHold());

        return result;

    }

    /**
     * Retrieve the neighboring node with the given name (as seen in the editor hireachy), or null if no such neighbor exists.
     */ 
    public MapNode GetNeighbourWithName(string name)
    {

        //Preconditions
        Assert.IsNotNull(name, "Precondtion Fail: The argument 'name' should not be null.");


        MapNode result = null;

        foreach (MapNode node in pathTo.Keys)
            if (node.name.Equals(name)) result = node;


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

        return result;

    }

    /**
     * Returns the path between the current and selected map node, with the configured movement direction.
     */
    public MovementPath GetPathTo(MapNode endNode)
    {

        MovementPath result = null;

        //Ensure the required node has an associated path and retrieve it.
        if (pathTo.ContainsKey(endNode)) {

            MovementPath path = pathTo[endNode];


            //Set the movement direction of the path based on which end the target node is on.
            if (endNode == path.EndNode.GetComponent<MapNode>()) pathTo[endNode].reverseEnumeration = false;
            else if (endNode == path.StartNode.GetComponent<MapNode>()) pathTo[endNode].reverseEnumeration = true;


            result = pathTo[endNode];

        }

        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

        return result;

    }

    /**
     * Fires a public event notifying other objects that the node has been reached.
     */ 
    public void NotifyReached()
    {

        NodeReached(this);


        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

    }

    private void OnMouseDown()
    {

        if(NodeSelected != null) NodeSelected(this);


        //Preconditions
        Assert.IsTrue(ClassInvariantsHold());

    }


    //Assertion Methods
    private bool RecordVariables()
    {
        
        //Deep clone pathsTo.
        pathToOnStart = new Dictionary<MapNode, MovementPath>();
        foreach (MapNode node in pathTo.Keys) pathToOnStart.Add(node, pathTo[node]);

        return true;

    }

    private bool ClassInvariantsHold()
    {

        //Ensure the keys and thier respective values in path start don't change, if they have been set in start.
        if (pathToOnStart != null)
        {
            foreach (MapNode node in pathTo.Keys)
                Assert.IsTrue(pathToOnStart.ContainsKey(node),
                    "Postcondition Fail: pathTo contains " + node + " which was not added on Start");
            foreach (MapNode node in pathToOnStart.Keys)
                Assert.IsTrue(pathTo.ContainsKey(node),
                       "Postcondition Fail: " + node + ", which was added on Start, is no longer contained in pathTo.");
            foreach (MapNode node in pathTo.Keys)
                Assert.IsTrue(pathTo[node] == pathToOnStart[node],
                    "Postcondition Fail " + node + " which was mapped to the value " + pathToOnStart[node] + " on Start, " +
                    "is now mapped to the value " + pathTo[node]);
        }

        return true;

    }

}
