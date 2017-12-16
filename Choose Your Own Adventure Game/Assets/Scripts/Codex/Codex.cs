using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A component that displays a tree of information to the screen.
 */
public abstract class Codex : MonoBehaviour {

    [SerializeField] protected CodexDirectory rootDirectory; //Directory node that is the ansestor of all other nodes.

    /**
     * Show the information in the codex.
     */
    public abstract void Display();

    /**
     * Hide the information in the codex.
     */
    public abstract void Hide();

    /**
     * Show the information in the given directory.
     */ 
    public abstract void OpenDirectory(CodexDirectory directory);

    /**
     * Show the given entry.
     */ 
    public abstract void OpenEntry(CodexEntry entry, string text);

    /**
     * Hide the information from the given directory.
     */
    public abstract void CloseDirectory(CodexDirectory directory);

    /**
     * Hide the information from thr given entry.
     */
    public abstract void CloseEntry(CodexEntry entry);

}
