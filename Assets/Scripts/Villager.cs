using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using Unity.VisualScripting.Dependencies.NCalc;
using XNode;

public class Villager : MonoBehaviour
{
    [SerializeField] private string itsName;
    public DialogueGraph[] dialogueGraphs;
    public int[] basicDialogueId = {0, 0};              // 기본 대사 범위(처음,끝)
    

    public bool neverSeenBefore = true;
    [SerializeField] private int curBDialogueState;     // 호출할 기본 대사 id
    [SerializeField] private VillagerName enumInfo;
    private Queue progressingQuest;
    private Interactable _interactable;
    private Animator _anim;
    private readonly int _returnToIdle = Animator.StringToHash("returnToIdle");
    private readonly int _startTalk = Animator.StringToHash("startTalk");
    private readonly int _embarrassed = Animator.StringToHash("embarrassed");

    
    //CharacterManager의 하위 객체. CharacterManager을 역참조한다
    private CharacterManager _manager;

    public string Name
    {
        get => itsName;
    }


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        itsName = Enum.GetName(typeof(VillagerName), enumInfo);
        if (transform.GetChild(0).TryGetComponent<Animator>(out var animator))
        {
            _anim = animator;
        }
        curBDialogueState = basicDialogueId[0]; //기본 대사 루틴을 첫번째로 설정
    }


    public int GetCurrentBDialogueState()
    {
        //현재 기본 대사 인덱스가 설정한 범위 안에 있는지 체크해서 값을 바꿈
        return curBDialogueState < basicDialogueId[1]? curBDialogueState++: curBDialogueState;
    }
 
   
    //상호작용
    public void Interact()
    {
        DialogueManager.Instance.ParseStart(dialogueGraphs[0]); //기본 대사 시작
        if (!_anim)
        {
            return;
        }
        _anim.ResetTrigger(_returnToIdle);
        _anim.SetTrigger(_startTalk);
    }
    
    //Idle 상태로 돌아가기
    public void ReturnToIdle()
    {
        if (!_anim)
        {
            return;
        }
        _anim.ResetTrigger(_startTalk);
        _anim.SetTrigger(_returnToIdle);
    }

    private void StartSweatAnim() => _anim.SetTrigger(_embarrassed);
    private void StopSweatAnim() => _anim.ResetTrigger(_embarrassed);

    public void SetupRequestingState()
    {
        // TODO: 주민 별로 표현 가짓수 넓히기
        StartSweatAnim();
    }

}

// [System.Serializable]
// public class DialogueRange
// {
//     public int startId;
//     public int endId;
// }
