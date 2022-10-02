using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using EnumTypes;
using Structs;
using System.Linq;

public class DataManager : Singleton<DataManager>
{
    public List<string> DialogueList = Enumerable.Repeat("null", 100).ToList(); 
    
    private BasicDialogueList _basicDialogueList;

    private void Start()
    {
        LoadDataFromJson();
        // Debug.Log(_basicDialogueList.basicDialogueData[1].text);
    }

    [ContextMenu("From Json Data")]
    private void LoadDataFromJson()
    {
        DialogueList = Enumerable.Repeat("null", 100).ToList(); 

        // DialogueList = new List<string>(100);
        // var path = Path.Combine(Application.dataPath, "Resources/Json/villagersData.json");
        
        //경로의 패스를 통해 정보를 읽어낸다.
        var path = Path.Combine(Application.dataPath, "Resources/Json/basicDialogueData.json");
        var jsonData = File.ReadAllText(path);
        _basicDialogueList = JsonUtility.FromJson<BasicDialogueList>(jsonData);
        foreach (var info in _basicDialogueList.basicDialogueData)
        {
            DialogueList[info.id] = info.text;
        }
        Debug.Log("Json 파일 읽기 완료");
    }

    //id 값에 따라 대사 return.
    public string GetDialogueData(int id = 0)
    {
        return DialogueList[id];
    }

    // public string ReturnBasicDialogue(int dialogueId)
    // {
    //     return bDialogueList.basicDialogueData[]
    // }
}
