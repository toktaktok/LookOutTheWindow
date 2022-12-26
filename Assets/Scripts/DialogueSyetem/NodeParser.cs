using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using EnumTypes;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using XNode;
using Debug = System.Diagnostics.Debug;

public class NodeParser : MonoBehaviour
{
    public DialogueGraph graph;
    private BaseNode bNode;
    // private Coroutine _parser;
    // public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogueText;
    // public Image speakerImage;
    
    private IEnumerator parser;

    private void Start()
    {
        graph = null;
        parser = null;
        GameManager.Input.keyAction += OnResumeNode;
    }

    public Coroutine NodeParse(DialogueGraph curGraph) //읽을 시작 노드를 찾는다.
    {
        graph = curGraph;
        foreach (BaseNode b in graph.nodes)
        {
            // Debug.Log(graph.nodes[0].GetType());
            if( graph.nodes[0].GetType().ToString() == "StartNode")
            {
                graph.current = b;
                bNode = b;
                break;
            }
        }

        if (!bNode)
        {
            return null;
        }
        parser = ParseNode();
        return StartCoroutine(parser);
    }

    private IEnumerator ParseNode() //노드의 정보를 읽어온다.
    {
        yield return new WaitForEndOfFrame();
        var b = graph.current; //그래프의 현재 노드.
        string data = b.GetString(); //노드 유형, 대사 받음
        string[] dataParts = data.Split('/');   //slash 기준으로 분할하기.
        //[0] - 노드 유형 / [1] - 노드 대사
        
        
        switch (dataParts[0])
        {
            case "Start": //시작 노드
                NextNode("exit"); // 포트에 연결된 다음 노드 확인.
                break;
            
            case "Choice":
                NextNode("exit" + dataParts[1]);
                break;
            
            case "Dialogue": //대사 노드
                dialogueText.text = dataParts[2]; // 대사
            
                if (b.IsBasicState()) //기본 상태인지 확인한다. -> 기본 대화 선택지 제시
                {
                    if (CharacterManager.Instance.Introduce()) //최초 대화 시, 선택 창 열기 X
                    {
                        break;
                    }
                    UIManager.Instance.OpenChoicePopup(); //선택 창 열기
                }
                
                //현재 대사 노드까지만 처리
                yield return null;
                break;
            
            case "Stop": //정지 노드
                UIManager.Instance.CloseDialoguePopup(); //대화창을 닫는다.
                parser = null;
                yield return null;
                break;
        }
    }

    //노드 계속해서 읽기
    private void OnResumeNode()
    {
        //parser가 null이 아닐 때만 실행되게? ui가 있을 때만 실행되게?
        if ( GameManager.Instance.curGameFlowState != GameFlowState.Interacting )
        {
            return;
        }

        if (GameManager.Instance.isInteracted)
        {
            GameManager.Instance.isInteracted = false;
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextNode("exit");
            StartCoroutine(ParseNode());
        }
    }

    
    
    private void NextNode(string fieldName)
    {
        if (parser != null)
        {
            StopCoroutine(parser); //진행 중이던 코루틴 정지
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
                return;
            }
        }
        UIManager.Instance.CloseDialoguePopup();
    }
}
