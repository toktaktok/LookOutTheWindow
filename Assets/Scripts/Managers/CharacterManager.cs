using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    //캐릭터에 저장된 정보를 관리하는 매니저. -> 현재 필요한가?
    [SerializeField] private List<Villager> curVillagerList = new List<Villager>();
    public Transform villagers;
    
    public Villager curInteractingVillager;

    private void Start()
    {
        UpdateVillagersList();
    }

    
    //주민과 처음 만났는지 확인
    public bool Introduce()
    {
        
        if (!curInteractingVillager.neverSeenBefore) //만난 적 있으면 소개 X
        {       
            return false;
        }
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
