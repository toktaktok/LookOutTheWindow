using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class Quest
{
    public static List<Quest> mainQuestList;
    private int _id;
    private bool _isActive;
    private string _title;
    private string _description;

    public List<Villager> relatedVillagerList
    {
        get;
        private set;
    }

    public Quest(int id, string title, string description)
    {
        
    }

    public void AddRelatedVillagers(Villager villager)
    {
        relatedVillagerList.Append(villager);
    }
    

}


