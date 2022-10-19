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
    public List<string> bDialogueList; //기본 대사 리스트
    public List<string> gDialogueList; //기본 대사 리스트
    
    // private BasicDialogueList _basicDialogueList;
    // private GossipDialogueList _gossipDialogueList;

    private void Start()
    {
        LoadDataFromJson();
        // Debug.Log(_basicDialogueList.basicDialogueData[1].text);
    }

    [ContextMenu("From Json Data")]
    private void LoadDataFromJson()
    {
        bDialogueList = Enumerable.Repeat("null", 300).ToList(); 
        gDialogueList = Enumerable.Repeat("null", 300).ToList();
        // var path = Path.Combine(Application.dataPath, "Resources/Json/villagersData.json");
        // BasicDialogueList basicDialogueList;
        // GossipDialogueList gossipDialogueList;
        
        //기본 대화 대사 데이터
        var path1 = Path.Combine(Application.dataPath, "Resources/Json/basicDialogueData.json");

        var jsonData1 = File.ReadAllText(path1);
        BasicDialogueList basicDialogueList = JsonUtility.FromJson<BasicDialogueList>(jsonData1);

        foreach (var info in basicDialogueList.dialogueData)
        {
            bDialogueList[info.id] = info.text;
        }

        //사담 대사 데이터
        var path2 = Path.Combine(Application.dataPath, "Resources/Json/gossipData.json");
        var jsonData2 = File.ReadAllText(path2);
        GossipDialogueList  gossipDialogueList = JsonUtility.FromJson<GossipDialogueList>(jsonData2);
        foreach (var info in gossipDialogueList.dialogueData)
        {
            gDialogueList[info.id] = info.text;
        }
        Debug.Log("Json 파일 읽기 완료");

    }

    //id 값에 따라 대사 return.
    public string GetDialogueData(int id = 0, string type = "b")
    {
        switch (type)
        {
            case "basic":
                return bDialogueList[id];
            case "gossip":
                return gDialogueList[id];
            default:
                return "찾는 대사 없음";

        }
    }

    // public string ReturnBasicDialogue(int dialogueId)
    // {
    //     return bDialogueList.basicDialogueData[]
    // }
}
