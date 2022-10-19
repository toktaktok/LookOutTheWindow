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
        // randInt = Random.Range(1, 3);
        return "Choice/" + Random.Range(1, 4);
    }
}