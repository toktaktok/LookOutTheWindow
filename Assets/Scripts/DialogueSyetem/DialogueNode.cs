using System.Collections.Generic;
using EnumTypes;
using UnityEngine;



[NodeWidth(290)]
public class DialogueNode : BaseNode
{
    [Input] public int entry;
    [Output] public int exit;
    
    // [SerializeField] private string speakerName = " ";
    [TextArea(3,  10)]
    public string dialogue = "";
    public Sprite sprite;
    public bool dialogueOptions = false;
    [SerializeField] private VillagerName curVillagerName = VillagerName.None;

    public VillagerName CurVillagerName
    {
        get => curVillagerName;
        set => curVillagerName = value;
    }

    public int dialogueId = 0;
    private Color prev;
    [SerializeField] private bool isBasicState;
    

    [System.Serializable]
    public class DialogueOption
    {
        public string dialogue;
        public string option;
        public int curDialogueId;

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
        if (isBasicState) //기본 대사 받기
        {
            // dialogue = DialogueManager.Instance.GetBasicDialogue(curVillagerEnumData.ToString());
            dialogue = DialogueManager.Instance.GetBasicDialogue();
        }
        else //일반 대사 받기
        {
            dialogue = DialogueManager.Instance.GetDialogue(dialogueId);
        }
        return "Dialogue/" + GetName() + "/" + dialogue; //노드 유형 + 주민 이름 + 대사
    }
    
    //주민의 이름 받기
    private string GetName()
    {
        return CurVillagerName.ToString();
    }

    public string GetDialogue()
    {
        return dialogue;
    }

    public override bool IsBasicState()
    {
        return isBasicState;
    }

    public void ToggleBasicState()
    {
        isBasicState = !isBasicState;
    }
    
    public override void GetValue()
    {
        return;
    }
}



