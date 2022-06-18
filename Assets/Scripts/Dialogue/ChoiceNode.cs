using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class ChoiceNode : BaseNode {

    [Input] public int entry;
    [Output] public int exit;
    // [Output] public int exit2;
    // [Output] public int exit3;
    
    public string speakerName = "default name";
    public string dialogueLine;
    
    public override string GetString()
    {
        // Debug.Log("DialogueNode/" + speakerName + "/" + dialogueLine);
        return "ChoiceNode/" + speakerName + "/" + dialogueLine;
    }
}