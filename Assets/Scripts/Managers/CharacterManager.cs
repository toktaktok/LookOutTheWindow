using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    //캐릭터에 저장된 정보를 관리하는 매니저. -> 현재 필요한가?
    [SerializeField] private List<Villager> curVillagerList = new List<Villager>();
    public Transform villagers;
    
    
    private void Start()
    {
        UpdateVillagersList();
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

    public string CurInteractingVillager()
    {
        //상호작용중인 주민 찾기
        int id = 1;
        return curVillagerList[id].ToString();
    }

    //찾아낸 주민이 출력할 기본대사 id 리턴.
    public int GetVillagerCurBDialogueState(string villagerName)
    {
        int iter = 0;
        foreach (var villager in curVillagerList)
        {
            if (villagerName == villager.name)
            {
                Debug.Log(curVillagerList[iter].basicDialogueId[0] +
                          curVillagerList[iter].GetCurrentBDialogueState());
                //현재 주민의 기본대사 시작 번호 + 기본 대사 상태
                return curVillagerList[iter].basicDialogueId[0] +
                       curVillagerList[iter].GetCurrentBDialogueState();
            }
            iter++;
        }
        return 0;
    }

}
