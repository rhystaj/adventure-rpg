using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Codex : ScriptableObject {

    [SerializeField] string root; //The root of the file system the codex is read from.

    private Dictionary<string, ScriptParser> codexTree; //The tree of codex entries and thier categories.

    private void OnEnable()
    {
        


    }

}
