using UnityEngine;
using UnityEngine.Assertions;

/**
* An node in the tree of data.
*/
public abstract class CodexNode : ScriptableObject
{
    public string nodeName;

    protected bool retrieved = false; //Whether or not the node has been retrieved.
    public bool Retrieved { get { return retrieved;  } }

    private CodexDirectory parentNode;

    private void OnEnable()
    {
        retrieved = false;
    }

    /**
     * Set the parent of the node to the given node, if the current node is contained within the given parent.
     */
    public bool SetParent(CodexDirectory parent)
    {
        //Preconditions
        Assert.IsNotNull(parent, "Precondition Fail: The 'parent' argument should not be null.");

        bool result = false;
        if (parent.NodeInDirectory(this))
        {
            parentNode = parent;
            result = true;
        }

        //Postconditions
        Assert.AreEqual<CodexDirectory>(parentNode, parent, "Postcondition Fail: The 'parentNode' field should be set to the 'parent' argument.");
        Assert.IsTrue(ClassInvariantsHold());

        return result;
    }

    /**
     * Returns whether the given CodexDirectory node is a parent of the node.
     */ 
    public bool HasParent(CodexDirectory parent) { return parent == parentNode; }

    /**
     * Implementation in subclasses get the information from the node. In this case return false 
     */
    public abstract void Retrieve(Codex codex);


    /**
     * Hide the information from the node.
     */
    public abstract void Return(Codex codex);


    //Assertion methods.
    private bool ClassInvariantsHold()
    {

        //If this node has a parent, its parent should contain it.
        Assert.IsFalse(parentNode != null && !parentNode.NodeInDirectory(this), "Postcondition Fail: " + name + "should be in the directory " + parentNode.name);

        return true;

    }

}
