using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
     * A branching node or category.
     */
[CreateAssetMenu]
public class CodexDirectory : CodexNode
{

    [SerializeField] CodexNode[] contents; //The items in the current directory.

    //Assetion fields.
    private CodexNode[] contentsOnEnable; //Represents the configuration of the contents array on enable.

    private void OnEnable()
    {
        //Assertion-only setup methods.
        Assert.IsTrue(RecordContents());

        //Preconditions
        Assert.IsNotNull(contents, "Precondition Fail: Contents should not be null.");

        //Make this the parent of each of the given nodes.
        foreach (CodexNode node in contents)
            node.SetParent(this);

        retrieved = false;

        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsFalse(retrieved, "Postcondition Fail: Retrieved should initiall be false.");
    }

    /**
     * Retrieve a list of the directory's children.
     */
    public List<CodexNode> GetChildren()
    {
        List<CodexNode> result = new List<CodexNode>(contents);

        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

        return result;
    }

    /**
     * Returns wheter a given node is a DIRECT child of the directory node.
     */
    public bool NodeInDirectory(CodexNode node) { 

        bool result = false;

        foreach (CodexNode n in contents)
        {
            if (n.Equals(node)){
                result = true;
                break;
            }
        }

        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());

        return result;

    }

    public override void Retrieve(Codex codex)
    {

        //Preconditions
        Assert.IsNotNull(codex, "Precondition Fail: The given codex should not be null");

        if (!retrieved)
        {
            codex.OpenDirectory(this);
            retrieved = true;
        }

        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsTrue(true, "Postcondition Fail: The field 'retrieved' should be true after this method has been called.");

    }

    public override void Return(Codex codex)
    {

        //Preconditions
        Assert.IsNotNull(codex, "Precondition Fail: The given codex should not be null");

        if (retrieved)
        {
            foreach (CodexNode node in contents) node.Return(codex); //Recursively return all children.
            codex.CloseDirectory(this);

            retrieved = false;
        }

        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsTrue(AllChildernNotRetrieved());
        Assert.IsFalse(retrieved, "Postcondition Fail: The field 'retrieved' should be false after this method has been called.");

    }

    //Assertion Methods
    private bool RecordContents()
    {
        Array.Copy(contents, contentsOnEnable, contents.Length);
        return true;
    }

    private bool ClassInvariantsHold()
    {
        Assert.AreEqual(contents, contentsOnEnable, "Postcondition Fail: The list of entries should never be changed at runtime.");
        Assert.IsTrue(ThisParentOfAllContentNodes());

        return true;
    }

    private bool ThisParentOfAllContentNodes()
    {
        foreach (CodexNode node in contents)
            Assert.IsTrue(node.HasParent(this), "Postcondition Fail: " + name + "should be the parent of " + node.name);
        return true;
    }

    private bool AllChildernNotRetrieved()
    {
        foreach (CodexNode node in contents)
            Assert.IsFalse(node.Retrieved, "Postcondition Fail: " + node.name + "should not be retrieved.");
        return true;
    }

}
