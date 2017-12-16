using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * A codex with a simple drop down menu to choose the category.
 */
public class DropCodex : Codex
{
    [SerializeField] Dropdown categoryDropdown; //The dropdown menu that will show the options for categories.

    private List<CodexNode> categories; //The list (in the order they appear) on entries in the dropdown.

    private void Start()
    {
        categories = rootDirectory.GetChildren();
        List<string> categoryNames = categories.ConvertAll(node => node.nodeName); //Get a list of all the node names to put in list.

        //Add names to the dropdown.
        categoryDropdown.AddOptions(categoryNames);
    }

    protected override void CloseDirectory(CodexDirectory directory)
    {
        throw new NotImplementedException();
    }

    protected override void CloseEntry(CodexEntry entry)
    {
        throw new NotImplementedException();
    }

    protected override void OpenDirectory(CodexDirectory directory)
    {

    }

    protected override void OpenEntry(CodexEntry entry, string text)
    {
        throw new NotImplementedException();
    }

    //Redundant in this implementation.
    public override void Display(){}
    public override void Hide(){}
}
