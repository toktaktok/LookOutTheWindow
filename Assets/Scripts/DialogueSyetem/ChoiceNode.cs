using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class ChoiceNode : BaseNode {

    [Input] public int entry;
    [Output] public int exit1;
    [Output] public int exit2;
    [Output] public int exit3;
    
    // public int randInt;
    // [TextArea]
    // public string dialogueLine;
    
    public override string GetString()
    {
        var randInt = Random.Range(1, 4);
        // Debug.Log(randInt);
        return "Choice/" + (randInt);
    }
}