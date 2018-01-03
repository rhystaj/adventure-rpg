using UnityEngine;
using UnityEngine.Assertions;

/**
    * A leaf or entry in the codex.
    */
[CreateAssetMenu]
public class CodexEntry : CodexNode
{
    [SerializeField] TextAsset script; //The text for the entry.

    private string scriptOnEnable; //The text of the script when the object is first enabled.

    private void OnEnable()
    {
        Assert.IsTrue(RecordScript()); //For assertions only.
        Assert.IsTrue(ClassInvariantsHold());
    }

    public override void Retrieve(Codex codex) {

        //Preconditions
        Assert.IsNotNull(codex, "The 'codex' argument should not be null.");

        if (!retrieved) {
            codex.OpenEntry(this, script.text);
            retrieved = true;
        }

        //Postconditions
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsTrue(retrieved, "The field 'retieved' should be true.");
        
    }

    public override void Return(Codex codex)
    {

        //Preconditions
        Assert.IsNotNull(codex, "The 'codex' argument should not be null.");

        if (retrieved) {
            codex.CloseEntry(this);
            retrieved = false;
        }

        //Postcondition
        Assert.IsTrue(ClassInvariantsHold());
        Assert.IsFalse(retrieved, "The field 'retieved' should be False.");

    }


    //Assertion methods.
    private bool RecordScript()
    {
        scriptOnEnable = script.text;
        return true;
    }

    private bool ClassInvariantsHold()
    {
        Assert.AreEqual<string>(scriptOnEnable, script.text, "The contents of the node should not change at runtime.");

        return true;
    }

}