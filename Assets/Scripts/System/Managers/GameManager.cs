using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        // Load Assets
        _characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
        _uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _minigameManager = GameObject.Find("MinigameManager").GetComponent<MinigameManager>();
        _cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();
        _dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        _questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    [SerializeField]
    private CharacterManager _characterManager;

    [SerializeField]
    private UIManager _uIManager;

    [SerializeField]
    private MinigameManager _minigameManager;

    [SerializeField]
    private CameraController _cameraController;

    [SerializeField]
    private DialogueManager _dialogueManager;

    [SerializeField]
    private QuestManager _questManager;

}
