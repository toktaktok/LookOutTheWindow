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

    private IEnumerator ParseNode() //노드의 정보를 읽어온다.
    {
        BaseNode b = graph.current;
        string data = b.GetString();
        string[] dataParts = data.Split('/');   // 노드의 데이터를 slash 기준으로 분할하기.

        
        if (dataParts[0] == "Start") // start 노드를 발견하면
        {
            NextNode("exit"); // 다음 노드가 존재하는지 확인한다.
        }
        else if (dataParts[0] == "DialogueNode") // dialogueNode를 발견하면
        {
            //Run Dialogue Processing
            // speaker.text = dataParts[1];
            if (b.IsBasicState()) // 기본 상태인지 확인한다. -> 기본 대화 선택지 제시
            {
                UIManager.Instance.OpenChoicePopup();
            }
            
            dialogue.text = dataParts[2];
            // speakerImage.sprite = b.GetSprite();
            
            // 다음 버튼이 눌릴 때까지 기다린다.
            // yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);
            yield return new WaitUntil(() => Keyboard.current.eKey.wasPressedThisFrame);
            // yield return new WaitUntil(() => Keyboard.current.eKey.wasReleasedThisFrame);
            NextNode("exit");   //현재 노드에 exit 포트가 있는가?(다음 노드가 존재하는가?)
        }
        else if (dataParts[0] == "Stop") // 현재 노드가 stop 노드라면
        {
            UIManager.Instance.CloseDialoguePopup(); //대화창을 닫는다.
            yield return null;
        }
    }

    public void ResumeNode()
    {
        
    }

    private void NextNode(string fieldName)
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
