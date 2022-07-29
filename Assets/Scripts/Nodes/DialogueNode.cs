using System.Collections;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using UnityEngine;
using XNode;



[NodeWidth(290)]
public class DialogueNode : BaseNode
{
    [Input] public int entry;
    [Output] public int exit;
    
    public string speakerName = " ";
    [TextArea(3,  10)]
    public string dialogue = "";
    public Sprite sprite;
    public bool dialogueOptions = false;
    
    
    private Color prev;
    [SerializeField] private bool isBasicState;
    

    [System.Serializable]
    public class DialogueOption
    {
        public string dialogue;
        public string option;

        public DialogueOption(string _dialogue, string _option)
        {
            dialogue = _dialogue;
            option = _option; 
        }
    }
    
    [SerializeField]
    public List<DialogueOption> dialogueOptionList = new List<DialogueOption>(5); 
    
    public override string GetString()
    {
        return "DialogueNode/" + speakerName + "/" + dialogue;
    }

    public override Sprite GetSprite()
    {
        return sprite;
    }

    public override bool IsBasicState()
    {
        return isBasicState;
    }

    public void ToggleBasicState()
    {
        isBasicState = !isBasicState;
    }
}



