using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumTypes;
using UnityEngine;


[System.Serializable]
public class Quest
{
    public List<string> relatedVillagerList
    {
        get;
        private set;
    }

    public string requestedVillager;
    
    public static List<Quest> mainQuestList;
    private int _id;
    private bool _isActive;
    private string _title;
    private string _description;



    //처음에 의뢰를 생성할 때는 부탁한 주민은 꼭 추가하기
    public Quest(int id, string title, string description, string villager)
    {
        _id = id;
        _title = title;
        _description = description;
        relatedVillagerList = new List<string>();
        requestedVillager = villager;
    }

    public void AddRelatedVillagers(VillagerEnumData villager)
    {
        // const bool ignoreCase = true;
        // var name = Enum.GetName(typeof(VillagerEnumData), villager);
        // // Debug.Log(Enum.TryParse(Name, ignoreCase, out interactableInfo));
        // if(Enum.TryParse())
        relatedVillagerList.Append(Enum.GetName(typeof(VillagerEnumData), villager));
    }
    

}


