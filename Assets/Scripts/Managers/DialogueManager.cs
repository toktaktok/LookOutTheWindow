using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DialogueManager : Singleton<DialogueManager>
{
    private NodeParser _nodeParser;

    public void Start()
    {
        _nodeParser = FindObjectOfType<NodeParser>().GetComponent<NodeParser>();

    }

    public void ParseStart(DialogueGraph graph) //해당하는 그래프를 받아 Parse 시작.
    {
        try
        {
            // Debug.Log(graph);
            _nodeParser.NodeParseStart(graph); //대사 노드 그래프를 찾아 보냄.
            UIManager.Instance.StartCoroutine( "OpenDialoguePopup" );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    // 그냥 대사 리턴, 일단 사담 대사 데이터에 모두 추가함!!
    public string GetDialogue(int id)
    {
        return DataManager.Instance.GetDialogueData(id, "gossip");
    }

    //기본 대사 아이디 받아서 리턴
    public string GetBasicDialogue()
    {
        var id = CharacterManager.Instance.GetVillagerCurBDialogueState();
        // var id = CharacterManager.Instance.GetVillagerCurBDialogueState(villagerName);
        return DataManager.Instance.GetDialogueData(id, "basic");
    }
    
    //사담하기
    public void GetGossip()
    {
        //캐릭터매니저에서 사담 그래프 얻기
        _nodeParser.NodeParseStart(CharacterManager.Instance.GetVillagerGraph(1));
        UIManager.Instance.CloseChoicePopup();
    }

    // 현재 상호작용하고 있는 오브젝트 or 주민에게 얻을 수 있는 증거가 있는지 파악한다. (interactable의 정보 확인, interacting 확인)
    public void GetEvidence()
    {
        
    }
}
