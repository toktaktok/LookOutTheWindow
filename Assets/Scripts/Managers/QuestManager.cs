using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : Singleton<QuestManager>
{
    
    // public List<Quest> questsInProgress; //진행 중인 퀘스트가 추가됨
    public Dictionary<string, Quest> questsInProgress;
    public Player player;
    public GameObject questWindow;
    public Text dialogue;
    

    public void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        questsInProgress = new Dictionary<string, Quest>();
    }

    // 진행 중인 의뢰 수를 비교하여 추가할 수 있는지 확인한다. bool return
    public bool AcceptRequest(string id, Quest newQuest)
    {
        if (questsInProgress.Count < 4)
        {
            questsInProgress.Add(id, newQuest);
            Debug.Log("의뢰 수락 완료");
            return true;
        }
        else
        {
            Debug.Log("의뢰 받을 수 없음!");
            // 의뢰를 받을 수 없다는 정보 제시하기
            return false;
        }
    }

    public bool CanTargetGiveEvidence()
    {
        // var villager = CharacterManager.Instance.CurInteractingVillager();
        // foreach (var quest in questsInProgress)
        // {
        //     if (quest.relatedVillagerList.Contains(villager)) // 이후, 주민이 여러 의뢰와 연관되어있을 경우에 조건 추가 필요
        //     {
        //         return true;
        //     }
        // }
        return false;
    }

    [ContextMenu("Add Quest")]
    //지금 쓸모 없음
    public void AddQuest()
    {
        var id = "1";
        var title = "";
        var description = "";
        var requestedVillager = VillagerName.None;
        var tdList = new List<string>();
        // Villager villager = null;

        EditorGUILayout.TextField("quest id", id);
        EditorGUILayout.TextArea("title", title);
        EditorGUILayout.TextField("description", description);
        EditorGUILayout.EnumPopup(requestedVillager);

        var newQuest = new Quest(id, title, description, Enum.GetName(typeof(VillagerName), requestedVillager), tdList);
    }
    
    public void AddQuestInProgress(string id)
    {
        var quest = DataManager.Instance.GetQuest(id);
        questsInProgress.Add(quest.Id, quest);
    }
    
    public void GetQuestInfo(string id)
    {
        var quest = DataManager.Instance.GetQuest(id);
        Debug.Log(quest.Title);
    }

}

