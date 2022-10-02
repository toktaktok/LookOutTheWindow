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
    public BaseNode bNode;
    // private Coroutine _parser;
    public IEnumerator parser;
    // public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogueText;
    // public Image speakerImage;


    private void Start()
    {
        graph = null;
        parser = null;
    }

    public Coroutine NodeParseStart(DialogueGraph curGraph)
    {
        graph = curGraph;
        foreach (BaseNode b in graph.nodes)
        {
            if( graph.nodes[0].GetType().ToString() == "StartNode")   //시작 노드를 찾기. (처음)
            {
                graph.current = b;
                bNode = b;
                break;
            }
        }
        
        parser = ParseNode();
        return StartCoroutine(parser);
    }

    private IEnumerator ParseNode() //노드의 정보를 읽어온다.
    {
        yield return new WaitForEndOfFrame();
        BaseNode b = graph.current; //그래프의 현재 노드.
        string data = b.GetString();
        string[] dataParts = data.Split('/');   // 노드의 데이터를 slash 기준으로 분할하기.
        
        
        if (dataParts[0] == "Start") // start 노드를 발견하면
        {
            NextNode("exit"); // 다음 노드가 존재하는지 확인.
        }
        else if (dataParts[0] == "DialogueNode") // dialogueNode를 발견하면
        {
            //Run Dialogue Processing
            if (b.IsBasicState()) // 기본 상태인지 확인한다. -> 기본 대화 선택지 제시
            {
                UIManager.Instance.OpenChoicePopup();
            }
            // Debug.Log(dataParts[2]);
            dialogueText.text = dataParts[2]; // 대사
            // speakerImage.sprite = b.GetSprite();
            
            // 다음 버튼이 눌릴 때까지 기다린다.
            // yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);
            // yield return new WaitUntil(() => Mouse.current.leftButton.wasReleasedThisFrame);
            
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E));

            // yield return new WaitUntil(() => Keyboard.current.eKey.wasReleasedThisFrame);
            NextNode("exit");   //현재 노드에 exit 포트가 있는가?(다음 노드가 존재하는가?)
        }
        else if (dataParts[0] == "Stop") // 현재 노드가 stop 노드라면
        {
            UIManager.Instance.CloseDialoguePopup(); //대화창을 닫는다.
            parser = null;
            graph = null;
            
            StopCoroutine(ParseNode());
            yield return null;
        }
    }

    public void ResumeNode()
    {
        
    }

    private void NextNode(string fieldName)
    {

        if (parser != null)
        {
            StopCoroutine(parser);
            parser = null;
        }

        foreach (NodePort p in graph.current.Ports)
        {
            //노드 뒤의 포트. 현재 노드의 포트가 전달된 포트의 이름과 동일한지 비교한다. -> exit
            if (p.fieldName == fieldName)
            {
                graph.current = p.Connection.node as BaseNode;
                parser = ParseNode();
                StartCoroutine(parser);
                break;
            }
            Debug.Log("다음 노드");

            // else
            // {
            //     Debug.Log("exit 포트 없음");
            //     UIManager.Instance.CloseDialoguePopup();
            //     break;
            // }
        }
        Debug.Log("다음 노드 없음");
        UIManager.Instance.CloseDialoguePopup();
    }
}
