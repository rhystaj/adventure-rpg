using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
     * A branching node or category.
     */
[CreateAssetMenu]
public class CodexDirectory : CodexNode
{

    [SerializeField] CodexNode[] contents; //The items in the current directory.

    private void OnEnable()
    {
        //Make this the parent of each of the given nodes.
        foreach (CodexNode node in contents)
            node.SetParent(this);

        retrieved = false;
    }

    /**
     * Retrieve a list of the directory's children.
     */
    public List<CodexNode> GetChildren()
    {
        return new List<CodexNode>(contents);
    }

    /**
     * Returns wheter a given node is a DIRECT child of the directory node.
     */
    public bool NodeInDirectory(CodexNode node)
    {
        foreach (CodexNode n in contents)
        {
            if (n.Equals(node)) return true;
        }

        return false;
    }

    public override void Retrieve(Codex codex)
    {

        Debug.Log(retrieved);

        if (retrieved) return;
        codex.OpenDirectory(this);

        retrieved = true;

    }

    public override void Return(Codex codex)
    {

        if (!retrieved) return;

        foreach (CodexNode node in contents) node.Return(codex); //Recursively return all children.
        codex.CloseDirectory(this);

        retrieved = false;
    }

}
