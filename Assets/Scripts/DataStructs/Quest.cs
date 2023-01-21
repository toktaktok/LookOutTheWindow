using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumTypes;
using UnityEngine;


[System.Serializable]
public class Quest
{
    public Dictionary<int, string> relatedVillagerList
    {
        get;
        private set;
    }

    public string requestedVillager;
    
    public static List<Quest> mainQuestList;
    private string _id;
    private bool _isActive;
    private string _title;
    private string _description;
    private List<string> _todoList;



    //처음에 의뢰를 생성할 때, 부탁한 주민은 꼭 추가하기
    public Quest(string id, string title,  string villager, string description, List<string> todoList)
    {
        _id = id;
        _title = title;
        requestedVillager = villager;
        _description = description;
        relatedVillagerList = new Dictionary<int, string>();
        _todoList = todoList;
    }

    public string Id
    {
        get { return _id; }
    }

    public string Title
    {
        get { return _title; }
    }

    public string Description
    {
        get { return _description; }
    }

    public List<string> TodoList
    {
        get { return _todoList;  }
    }

    public void AddRelatedVillagers(VillagerName villager)
    {
        var id = (int)villager;
        if (!relatedVillagerList.ContainsKey(id))
        {
            relatedVillagerList.Add((int)villager, villager.ToString());
        }

    }
    public string QuestInfo()
    {
        foreach (var list in _todoList)
        {
            Debug.Log( list );
        }

        return _title + " / "+ _description;
    }

}


