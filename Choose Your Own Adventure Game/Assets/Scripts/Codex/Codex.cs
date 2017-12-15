using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A component that displays a tree of information to the screen.
 */
public abstract class Codex : MonoBehaviour {

    [SerializeField] CodexDirectory rootDirectory; //Directory node that is the ansestor of all other nodes.

    /**
     * Show the information in the codex.
     */
    public virtual void Display()
    {
        rootDirectory.Retrieve();
    }

    /**
     * Hide the information in the codex.
     */
    public virtual void Hide()
    {
        rootDirectory.Return();
    }

    /**
     * Show the information in the given directory.
     */ 
    protected abstract void OpenDirectory(CodexDirectory directory, CodexNode[] subnodes);

    /**
     * Show the given entry.
     */ 
    protected abstract void OpenEntry(CodexEntry entry, string text);

    /**
     * Hide the information from the given directory.
     */
    protected abstract void CloseDirectory(CodexDirectory directory);

    /**
     * Hide the information from thr given entry.
     */
    protected abstract void CloseEntry(CodexEntry entry);

    /**
     * An node in the tree of data.
     */
    public abstract class CodexNode : ScriptableObject
    {
        [SerializeField] protected string nodeName;

        protected Codex codex;

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
        public abstract void Retrieve();

        /**
         * Hide the information from the node.
         */ 
        public abstract void Return();

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

        public override void Retrieve()
        {
            codex.OpenDirectory(this, contents);
        }

        public override void Return()
        {
            codex.CloseDirectory(this);
        }
    }

    /**
     * A leaf or entry in the codex.
     */
    [CreateAssetMenu]
    public class CodexEntry : CodexNode
    {
        [SerializeField] TextAsset script; //The text for the entry.

        public override void Retrieve()
        {
            codex.OpenEntry(this, script.text);
        }

        public override void Return()
        {
            codex.CloseEntry(this);
        }
    }

}
