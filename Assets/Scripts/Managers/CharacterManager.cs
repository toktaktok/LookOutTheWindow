using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    
    //캐릭터들을 조작하는 매니저.
    [SerializeField] private List<Villager> curVillagerList;
    public Transform villagers;
    
    public Villager curInteractingVillager;
    private Animator _curInteractingVAnimator;
    
    private void Start()
    {
        curVillagerList = new List<Villager>();
        UpdateVillagersList();
    }

    
    //주민과 대화 시 처음 체크
    public bool FirstInteractCheck()
    {
        //현재 대화하는 주민의 애니메이터로 초기화하기
        if (curInteractingVillager.TryGetComponent<Animator>(out var animator))
        {
            _curInteractingVAnimator = animator;
        }
        
        //만난 적 있으면 소개 X
        if (!curInteractingVillager.neverSeenBefore)
        {       
            return false;
        }
        
        //없으면 소개
        switch (curInteractingVillager.itsName)
        {
            case "Zig":
                return true;
            default: 
                curInteractingVillager.neverSeenBefore = false;
                return true;
        }
    }

    //주민이 출력할 기본대사 id 리턴.
    public int GetVillagerCurBDialogueState()
    {
        return curInteractingVillager.GetCurrentBDialogueState();
    }

    //주민이 줄 그래프 받기(일단 0: 기본, 1: 사담)
    public DialogueGraph GetVillagerGraph(int id)
    {
        return curInteractingVillager.dialogueGraphs[id];
    }
    
    
    //현재 존재하는 주민 리스트 업데이트
    private void UpdateVillagersList()
    {
        for (var i = 0; i < villagers.childCount; i++)
        {
            if (villagers.GetChild(i).TryGetComponent(out Villager newVillager))
            {
                curVillagerList.Add(newVillager);
            }
        }
    }

    public void ChangeEmotion(string emotion)
    {
        switch (emotion)
        {
            case "Joy":
                break;
            case "Angry":
                break;
            case "Embarrassed":
                _curInteractingVAnimator.SetTrigger("Embarrassed");
                break;
            default:
                _curInteractingVAnimator.ResetTrigger("Embarrassed");
                break;
        }
    }
    public void StopTalk()
    {
        curInteractingVillager.ReturnToIdle();
    }
    //현재 주민 인덱스 찾기
    // private int SearchVillagerId(string name)
    // {
    //     var iter = 0;
    //     foreach (var villager in curVillagerList)
    //     {
    //         if (name == villager.Name)
    //         {
    //             //현재 주민의 기본대사 시작 번호 + 기본 대사 상태
    //             return iter;
    //         }
    //
    //         iter++;
    //     }
    //
    //     Debug.Log("주민 없음");
    //     return 999;
    // }

    // private Villager SearchVillager(string name)
    // {
    //     foreach (var villager in curVillagerList)
    //     {
    //         if (name == villager.Name)
    //         {
    //             //현재 주민의 기본대사 시작 번호 + 기본 대사 상태
    //             return villager;
    //         }
    //     }
    //     Debug.Log("주민 없음");
    //     return null;
    // }

    
}
