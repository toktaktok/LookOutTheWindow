using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using XNode;
using UnityEngine.InputSystem;

public class NodeParser : MonoBehaviour
{
    public DialogueGraph graph;
    // private Coroutine _parser;
    private IEnumerator _parser;
    // public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogue;
    // public Image speakerImage;
    

    public Coroutine NodeParseStart()
    {
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
            
            dialogue.text = dataParts[2];
            // speakerImage.sprite = b.GetSprite();
            yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);
            yield return new WaitUntil(() => Mouse.current.leftButton.wasReleasedThisFrame);
            NextNode("exit");   //현재 노드에 exit 포트가 있는가?(다음 노드가 존재하는가?)
        }
        else if (dataParts[0] == "ChoiceNode")
        {
            dialogue.text = dataParts[2];
            
            /* ui매니저에서 선택지 팝업을 띄우도록 하기.
             * 가능한 선택지 띄우는 법: Interactable에서 가능한 선택지를 확인.
             * 여기서 확인해서 보낼거임? Interactable에서 확인해서 보낼 거임?
             * ui에서 선택한 답은 어떤 노드로 이동할지 알려준다.
            */
            
            UIManager.Instance.OpenChoicePopup();
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
