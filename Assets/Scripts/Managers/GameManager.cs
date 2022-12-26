using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

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
        if (FindObjectOfType<InputManager>())
        {
            _inputManager = FindObjectOfType<InputManager>().GetComponent<InputManager>();
        }
        if (FindObjectOfType<CameraController>())
        {
            _cameraController = FindObjectOfType<CameraController>().GetComponent<CameraController>();
        }
        if (FindObjectOfType<DataManager>())
        {
            _dataManager = FindObjectOfType<InputManager>().GetComponent<DataManager>();
        }
        if (FindObjectOfType<UIManager>())
        {
            _uIManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
        }
        // if (GameObject.Find("CharacterManager").TryGetComponent<CharacterManager>(out var characterManager))
        // {
        //     _characterManager = characterManager;
        // }

        // if (GameObject.Find("MiniGameManager").TryGetComponent<MiniGameManager>(out var miniGameManager))
        // {
        //     _miniGameManager = miniGameManager;
        // }

        // if (GameObject.Find("DialogueManager").TryGetComponent<DialogueManager>(out var dialogueManager))
        // {
        //     _dialogueManager = dialogueManager;
        // }
        // if (GameObject.Find("QuestManager").TryGetComponent<QuestManager>(out var questManager))
        // {
        //     _questManager = questManager;
        // }
    }

    private void Update()
    {
        _inputManager.OnUpdate();
    }
    

    public GameFlowState curGameFlowState = GameFlowState.InGame;
    public bool isInteracted = false;
    
    [SerializeField]
    private InputManager _inputManager;
    
    [SerializeField]
    private CameraController _cameraController;
    
    [SerializeField]
    private UIManager _uIManager;
    
    [SerializeField]
    private DataManager _dataManager;
    
    [SerializeField]
    private CharacterManager _characterManager;


    [SerializeField]
    private MiniGameManager _miniGameManager;


    [SerializeField]
    private DialogueManager _dialogueManager;

    [SerializeField]
    private QuestManager _questManager;
    
 



    public static InputManager Input
    {
        get => Instance._inputManager;
    }
    
}
