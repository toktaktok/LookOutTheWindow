using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private NodeParser _nodeParser;

    public void ParseStart(DialogueGraph graph) //해당하는 그래프를 받아 Parse 시작.
    {
        try
        {
            _nodeParser.NodeParseStart(graph); //대사 노드 그래프를 찾아 보냄.
            UIManager.Instance.StartCoroutine("OpenDialoguePopup");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    // 그냥 대사 리턴
    public string GetDialogue(int id)
    {
        return DataManager.Instance.GetDialogueData(id);
    }

    //기본 대사 아이디 받아서 리턴
    public string GetBasicDialogue(string villagerName)
    {
        int id = CharacterManager.Instance.GetVillagerCurBDialogueState(villagerName);
        return GetDialogue(id);
    }
    
    //사담하기
    public void GetGossip()
    {
        //캐릭터매니저에서 이 주민이 할 사담이 있는지 확인
        //캐릭터매니저에서 랜덤으로 말할 사담을 말함
    }

    // 현재 상호작용하고 있는 오브젝트 or 주민에게 얻을 수 있는 증거가 있는지 파악한다. (interactable의 정보 확인, interacting 확인)
    public void GetEvidence()
    {
        
    }
}
