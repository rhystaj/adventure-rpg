using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* An node in the tree of data.
*/
public abstract class CodexNode : ScriptableObject
{
    public string nodeName;

    protected bool retrieved; //Whether or not the node has been retrieved.

    private CodexDirectory parentNode;

    /**
     * Set the parent of the node to the given node, if the current node is contained within the given parent.
     */
    public bool SetParent(CodexDirectory parent)
    {
        if (parent.NodeInDirectory(this))
        {
            parentNode = parent;
            return true;
        }
        return false;
    }

    /**
     * Implementation in subclasses get the information from the node. In this case return false 
     */
    public abstract void Retrieve(Codex codex);


    /**
     * Hide the information from the node.
     */
    public abstract void Return(Codex codex);

}
