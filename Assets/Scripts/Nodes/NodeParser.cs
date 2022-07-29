using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using XNode;

public class NodeParser : MonoBehaviour
{
    public DialogueGraph graph;
    // private Coroutine _parser;
    private IEnumerator _parser;
    // public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogue;
    // public Image speakerImage;


    private void Start()
    {
        graph = null;
        _parser = null;
    }

    public Coroutine NodeParseStart(DialogueGraph curGraph)
    {
        graph = curGraph;
        foreach (BaseNode b in graph.nodes)
        {
            if(b.GetString() == "Start")   //시작 노드를 찾아오기.
            {
                graph.current = b;
                break;
            }
        }
        
        _parser = ParseNode();
        return StartCoroutine(_parser);
    }

    IEnumerator ParseNode() //노드의 정보를 읽어온다.
    {
        BaseNode b = graph.current;
        string data = b.GetString();
        string[] dataParts = data.Split('/');

        if (dataParts[0] == "Start")
        {
            NextNode("exit");
        }
        else if (dataParts[0] == "DialogueNode")
        {
            //Run Dialogue Processing
            // speaker.text = dataParts[1];
            if (b.IsBasicState())
            {
                UIManager.Instance.OpenChoicePopup();
            }
            
            dialogue.text = dataParts[2];
            // speakerImage.sprite = b.GetSprite();
            yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);
            yield return new WaitUntil(() => Mouse.current.leftButton.wasReleasedThisFrame);
            NextNode("exit");   //현재 노드에 exit 포트가 있는가?(다음 노드가 존재하는가?)
        }
        else if (dataParts[0] == "Stop")
        {
            UIManager.Instance.CloseDialoguePopup();
            yield return null;

        }
    }

    public void NextNode(string fieldName)
    {

        if (_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }

        foreach (NodePort p in graph.current.Ports)
        {
            //노드 뒤의 포트. 현재 노드의 포트가 전달된 포트의 이름과 동일한지 비교한다. -> exit
            if (p.fieldName == fieldName)
            {
                graph.current = p.Connection.node as BaseNode;
                break;
            }
            // else
            // {
            //     UIManager.Instance.CloseDialoguePopup();
            // }
        }

        _parser = ParseNode();
        StartCoroutine(_parser);
    }
}
