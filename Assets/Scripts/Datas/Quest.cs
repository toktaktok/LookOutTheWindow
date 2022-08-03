using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Quest
{
    public static List<Quest> mainQuestList;
    public int id;
    public bool isActive;
    public string title;
    public string description;
    public List<Villager> relatedVillagers;
}
