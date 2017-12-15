using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A codex with a simple drop down menu to choose the category.
 */
public class DropCodex : Codex
{
    public override void Display()
    {
        base.Display();
    }

    public override void Hide()
    {
        base.Hide();
    }

    protected override void CloseDirectory(CodexDirectory directory)
    {
        throw new NotImplementedException();
    }

    protected override void CloseEntry(CodexEntry entry)
    {
        throw new NotImplementedException();
    }

    protected override void OpenDirectory(CodexDirectory directory, CodexNode[] subnodes)
    {
        throw new NotImplementedException();
    }

    protected override void OpenEntry(CodexEntry entry, string text)
    {
        throw new NotImplementedException();
    }
}
