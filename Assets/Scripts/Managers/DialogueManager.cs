using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private NodeParser _nodeParser;

    public void ParseStart(DialogueGraph graph)
    {
        _nodeParser.NodeParseStart(graph);
        UIManager.Instance.OpenDialoguePopup();
    }
}
