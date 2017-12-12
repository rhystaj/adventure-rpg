using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Codex : ScriptableObject {

    [SerializeField] string root; //The root of the file system the codex is read from.



    private void OnEnable()
    {
        //Read from the file system and load into graph.
    }

}
