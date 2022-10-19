using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using Unity.VisualScripting.Dependencies.NCalc;
using XNode;

public class Villager : MonoBehaviour
{
    public string itsName;
    public DialogueGraph[] dialogueGraphs;
    public int[] basicDialogueId = {0, 0}; // 기본 대사 범위(처음,끝)

    public bool neverSeenBefore = true;
    [SerializeField] private int curBDialogueState; // 호출할 기본 대사 id
    [SerializeField] private VillagerEnumData EnumInfo;
    private Interactable interactable;

    
    //CharacterManager의 하위 객체. CharacterManager을 역참조한다
    private CharacterManager _manager;

    public string Name
    {
        get => itsName;
    }


    private void Start()
    {
        Init();
        curBDialogueState = basicDialogueId[0];
    }

    private void Init()
    {
        itsName = Enum.GetName(typeof(VillagerEnumData), EnumInfo);
    }


    public int GetCurrentBDialogueState()
    {

        return curBDialogueState < basicDialogueId[1]? curBDialogueState++: curBDialogueState;
    }
 
   

    public void Interact()
    {
        DialogueManager.Instance.ParseStart(dialogueGraphs[0]); //기본 대사 시작
        // Debug.Log(itsName);
   
        // UIManager.Instance.OpenDialoguePopup(interactable.DialogueGraphs[0]);
        // interactable.CheckMiniGame();
    }

}
// [System.Serializable]
// public class DialogueRange
// {
//     public int startId;
//     public int endId;
// }
