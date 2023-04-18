using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using EnumTypes;
using Structs;
using System.Linq;
using SimpleJSON;
// using newtonsoft;

public class SerializedDictionary : SerializableDictionary<string, string>{}

public class DataManager : Singleton<DataManager>
{
    public List<string> bDialogueList; //기본 대사 리스트
    public List<string> gDialogueList; //기본 대사 리스트
    public Dictionary<string, Quest> qList = new Dictionary<string, Quest>();
    public SerializedDictionary sDialogueDic;

    private void Start()
    {
        LoadDataFromJson();
        
    }

    [ContextMenu("From Json Data")]
    private void LoadDataFromJson()
    {
        bDialogueList = Enumerable.Repeat("null", 300).ToList();
        gDialogueList = Enumerable.Repeat("null", 300).ToList();

        //기본 대화 대사 데이터
        var path1 = Path.Combine(Application.dataPath, "Resources/Json/basicDialogueData.json");
        var jsonData1 = File.ReadAllText(path1);
        BasicDialogueList basicDialogueList = JsonUtility.FromJson<BasicDialogueList>(jsonData1);

        foreach (var info in basicDialogueList.dialogueData)
        {
            bDialogueList[info.id] = info.text;
        }

        //잡담 대사 데이터
        var path2 = Path.Combine(Application.dataPath, "Resources/Json/generalData.json");
        var jsonData2 = File.ReadAllText(path2);
        DialogueList  dialogueList = JsonUtility.FromJson<DialogueList>(jsonData2);
        foreach (var info in dialogueList.dialogueData)
        {
            gDialogueList[info.id] = info.text;
        }
        
        //의뢰 데이터 추가
        QuestJsonParse();

        // Debug.Log("Json 파일 읽기 완료");

    }

    public void JsonParse()
    {
        var root = ParseJson("storyData", "storyDialogues");
        foreach (var item in root)
        {
            sDialogueDic.Add(item.Key, item.Value);
        }

    }
    
    [ContextMenu("Quest Json Parse")]
    public void QuestJsonParse()
    {
        var root = ParseJson("questData", "quests");
        
        foreach (var item in root)
        {
            //투두리스트 묶기
            var tdList = new List<string>();
            
            for (int i = 0; i < 5; i++)
            {
                if (item.Value["toDo" + (i + 1)] == "null")
                {
                    break;
                }
                tdList.Add(item.Value["toDo" + (i + 1)]);
            }
            
            //퀘스트 묶기
            var quest = new Quest(item.Value["id"], item.Value["title"], item.Value["requestedVillager"],
                item.Value["description"], tdList);
            qList.Add( item.Value["id"], quest);
        }
        
        foreach (var info in qList)
        {
            // Debug.Log( info.Key );
        }
        
        QuestManager.Instance.AddQuestInProgress("1");
        QuestManager.Instance.GetQuestInfo("1");
    }
    
    public Quest GetQuest(string id)
    {
        return qList[id];
    }
    
    //id 값에 따라 대사 return.
    public string GetDialogueData(int id = 0, string type = "b")
    {
        switch (type)
        {
            case "basic":
                return bDialogueList[id];
            case "general":
                return gDialogueList[id];
            default:
                return "찾는 대사 없음";

        }
    }

    
    public JSONNode ParseJson(string filePath, string fileName)
    {
        var path = Path.Combine(Application.dataPath, "Resources/Json/", filePath + ".json");
        var jsonData =  File.ReadAllText(path);
        JSONNode text = JSON.Parse(jsonData);
        return text[fileName];

    }



}
