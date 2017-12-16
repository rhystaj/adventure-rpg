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
    [SerializeField] DynamicButtonGroup optionsGroup; //The button group where the entry options will be displayed.

    private List<CodexNode> categories; //The list (in the order they appear) on entries in the dropdown.
    private int currentOption = 0;

    private void Start()
    {
        categories = rootDirectory.GetChildren();
        List<string> categoryNames = categories.ConvertAll(node => node.nodeName); //Get a list of all the node names to put in list.

        //Add names to the dropdown.
        categoryDropdown.AddOptions(categoryNames);

        categories[0].Retrieve(this);

    }

    public override void CloseDirectory(CodexDirectory directory)
    {
        optionsGroup.Clear();
    }

    public override void CloseEntry(CodexEntry entry)
    {
        throw new NotImplementedException();
    }

    public override void OpenDirectory(CodexDirectory directory)
    {
        int count = 0;
        foreach(CodexNode node in directory.GetChildren())
        {
            optionsGroup.AddOption(count++, node.nodeName, () => { });
        }

        optionsGroup.DrawGroup();

    }

    public override void OpenEntry(CodexEntry entry, string text)
    {
        throw new NotImplementedException();
    }

    /**
     * The response to when an option from the drop-down list is selected.
     */ 
    public void OnOptionNumberIsSelected(int option)
    {
        categories[currentOption].Return(this);
        categories[option].Retrieve(this);

        currentOption = option;
    }

    //Redundant in this implementation.
    public override void Display(){}
    public override void Hide(){}
}
