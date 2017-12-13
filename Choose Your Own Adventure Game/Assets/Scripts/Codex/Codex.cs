using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A component that displays a tree of information to the screen.
 */
public abstract class Codex : MonoBehaviour {

    /**
     * Show the information in the codex.
     */ 
    public void Display()
    {

    }

    /**
     * Hide the information in the codex.
     */ 
    public void Hide()
    {

    }

    protected abstract void OpenDirectory(CodexNode subnodes);

    protected abstract void OpenEntry(TextAsset script);

    /**
     * An node in the tree of data.
     */
    public abstract class CodexNode : ScriptableObject
    {
        [SerializeField] protected string nodeName;

        private CodexDirectory parentNode;

        /**
         * Set the parent of the node to the given node, if the current node is contained within the given parent.
         */ 
        public bool SetParent(CodexDirectory parent) {
            if(parent.NodeInDirectory(this))
            {
                parentNode = parent;
                return true;
            }
            return false;
        }

        /**
         * Get the information from the node.
         */ 
        protected abstract void Retrieve();

    }

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
        }

        /**
         * Returns wheter a given node is a DIRECT child of the directory node.
         */ 
        public bool NodeInDirectory(CodexNode node)
        {
            foreach(CodexNode n in contents)
            {
                if (n.Equals(node)) return true;
            }

            return false;
        }

        protected override void Retrieve()
        {
            throw new NotImplementedException();
        }
    }

    /**
     * A leaf or entry in the codex.
     */
    [CreateAssetMenu]
    public class CodexEntry : CodexNode
    {
        [SerializeField] TextAsset script; //The text for the entry.

        protected override void Retrieve()
        {
            throw new NotImplementedException();
        }
    }

}
