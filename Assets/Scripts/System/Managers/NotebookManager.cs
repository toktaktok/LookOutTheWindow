using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NotebookManager : Singleton<NotebookManager>
{
    private Quest mainQuest;
    private LinkedList<Quest> subQuestList;
    public GameObject coverPage;
    public GameObject questPage;

    public void AddQuest(Quest quest)
    {
        subQuestList.AddLast(quest);
    }

    // public void RemoveQuest(Quest quest)
    // {
    //     foreach (var iter in subQuestList)
    //     {
    //         if (iter.id == quest.id)
    //         {
    //             subQuestList.Remove(quest);
    //         }
    //     }
    // }

    // public IEnumerable<Quest> RemoveQuest(Quest deletingQuest)
    // {
    //     // return from Quest quest in subQuestList
    //     //     where deletingQuest.id == quest.id
    //     // subQuestList.Remove(deletingQuest);
    //
    // }
}
