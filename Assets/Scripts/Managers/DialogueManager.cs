using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private NodeParser _nodeParser;

    public void ParseStart(DialogueGraph graph)
    {
        try
        {
            _nodeParser.NodeParseStart(graph);
            UIManager.Instance.OpenDialoguePopup();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    
    }

    // 현재 상호작용하고 있는 오브젝트 or 주민에게 얻을 수 있는 증거가 있는지 파악한다. (interactable의 정보 확인, interacting 확인)
    public void CheckQuest()
    {
        
    }
}
