using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using XNode;

public class Villager : MonoBehaviour
{

    [SerializeField] private string itsName;
    
    [SerializeField] public int[] basicDialogueId; // 기본 대사 범위(처음,끝)
    [HideInInspector] private int curBDialogueState = 0; // 호출할 기본 대사 id
    // [SerializeField] private InteractableData interactableInfo;
    public DialogueGraph[] dialogueGraphs;
    [SerializeField] private Interactable interactable;
    private VillagerEnumData itsInfo;

    //CharacterManager의 하위 객체. CharacterManager을 역참조한다
    private CharacterManager _manager;
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        itsName = interactable.Name;
        interactable.GetEnumName();
    }
    public void SwitchBasicDialogueID()
    {
        
    }

    public void PlusCurBDialogueState() => curBDialogueState++;

    public int GetCurrentBDialogueState()
    {
        return curBDialogueState;
    }
 

    public void Check()
    {
        DialogueManager.Instance.ParseStart(dialogueGraphs[0]); //기본 대사 시작
        // UIManager.Instance.OpenDialoguePopup(interactable.DialogueGraphs[0]);
        // Debug.Log("주민 id : " + interactable.Id);
        // Debug.Log("주민 이름 : " + itsName);
        // Debug.Log("miniGame Id : " + interactable.Minigameid);
        // interactable.CheckMiniGame();
    }

}
// [System.Serializable]
// public class DialogueRange
// {
//     public int startId;
//     public int endId;
// }
