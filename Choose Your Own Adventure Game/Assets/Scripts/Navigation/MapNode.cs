using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour {

    [SerializeField] MovementPath[] pathsFrom; //The paths that can be taken from the node.

    //The paths that can be taken from the node to reach another node.
    private Dictionary<MapNode, MovementPath> pathTo = new Dictionary<MapNode, MovementPath>(); 

    public delegate void NodeInteraction(MapNode node); //  A delegate for when a node is interacted with.
    public static event NodeInteraction NodeSelected; //    An event that is fired when a node is selected.
    public static event NodeInteraction NodeReached; //     An event that is fired when the node is reached by the player.

    private void Start()
    {
           
        //Populate the pathsTo dictionary with the nodes at the ends of the paths in pathsFrom, mapped to thier respective paths.
        foreach(MovementPath path in pathsFrom)
        {
            //Get the map node components which should be assigned to nodes at either ends of the path.
            MapNode pathStart = path.Start.GetComponent<MapNode>();
            MapNode pathEnd = path.End.GetComponent<MapNode>();


            //Both the start and end nodes should have a map node component - throw an error if this is not the case.
            if (pathStart == null)
                throw new Exception(path.Start.name + "should have a MapNode compenet assigned to it, as it is the start of a path.");
            if(pathEnd == null)
                throw new Exception(path.End.name + 
                                    "should have a MapNode component assigned to it, as it is the end of a path.");


            //Add pathStart or pathEnd to pathTo, depending in which one is not this node.
            if (pathStart != this)
                pathTo.Add(pathStart, path);
            else if (pathEnd != this)
                pathTo.Add(pathEnd, path);
            else
                throw new Exception(path.name + "should not be assigned as a path from " + name + ", as it does not start or end with it.");

            Debug.Assert((pathTo.ContainsKey(pathStart) || pathTo.ContainsKey(pathEnd)) && !pathTo.ContainsKey(this),
                         "The pathTo field of" + name + "should contain the node at the other end of the path " + path.name + " and not itself");
            Debug.Assert(pathTo.ContainsValue(path), "The pathTo field of" + name + "should contain the path " + path.name);

        }

    }

    //Make all paths from this node untraversable.
    public void CloseAllPaths()
    {
        foreach (MovementPath path in pathTo.Values) path.traversable = false;
    }

    /**
     * Retrieve the neighboring node with the given name (as seen in the editor hireachy), or null if no such neighbor exists.
     */ 
    public MapNode GetNeighbourWithName(string name)
    {
        foreach (MapNode node in pathTo.Keys)
            if (node.name.Equals(name)) return node;
        return null;
    }

    /**
     * Returns the path between the current and selected map node, with the configured movement direction.
     */ 
    public MovementPath GetPathTo(MapNode endNode)
    {

        //Ensure the required node has an associated path and retrieve it.
        if (!pathTo.ContainsKey(endNode)) return null;
        MovementPath path = pathTo[endNode];


        //Set the movement direction of the path based on which end the target node is on.
        if (endNode == path.End.GetComponent<MapNode>()) pathTo[endNode].reverseEnumeration = false;
        else if (endNode == path.Start.GetComponent<MapNode>()) pathTo[endNode].reverseEnumeration = true;

        
        return pathTo[endNode];
    }

    /**
     * Fires a public event notifying other objects that the node has been reached.
     */ 
    public void NotifyReached()
    {
        if(NodeReached != null) NodeReached(this);
    }

    private void OnMouseDown()
    {
        if(NodeSelected != null) NodeSelected(this);   
    }

}
