using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    * A leaf or entry in the codex.
    */
[CreateAssetMenu]
public class CodexEntry : CodexNode
{
    [SerializeField] TextAsset script; //The text for the entry.

    public override void Retrieve(Codex codex)
    {
        if (retrieved) return;
        codex.OpenEntry(this, script.text);

        retrieved = true;

    }

    public override void Return(Codex codex)
    {
        if (!retrieved) return;
        codex.CloseEntry(this);

        retrieved = false;

    }
}