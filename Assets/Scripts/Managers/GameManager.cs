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
        if (GameObject.Find("InputManager").TryGetComponent<InputManager>(out var inputManager))
        {
            _inputManager = inputManager;
        }
        if (GameObject.Find("DataManager").TryGetComponent<DataManager>(out var dataManager))
        {
            _dataManager = dataManager;
        }
        if (GameObject.Find("CharacterManager").TryGetComponent<CharacterManager>(out var characterManager))
        {
            _characterManager = characterManager;
        }
        if (GameObject.Find("UIManager").TryGetComponent<UIManager>(out var uiManager))
        {
            _uIManager = uiManager;
        }
        if (GameObject.Find("MiniGameManager").TryGetComponent<MiniGameManager>(out var miniGameManager))
        {
            _miniGameManager = miniGameManager;
        }
        if (GameObject.Find("CameraController").TryGetComponent<CameraController>(out var cameraController))
        {
            _cameraController = cameraController;
        }
        if (GameObject.Find("DialogueManager").TryGetComponent<DialogueManager>(out var dialogueManager))
        {
            _dialogueManager = dialogueManager;
        }
        if (GameObject.Find("QuestManager").TryGetComponent<QuestManager>(out var questManager))
        {
            _questManager = questManager;
        }


        // _characterManager = GameObject.Find("CharacterManager").GetComponent<CharacterManager>();
        // _uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        // _miniGameManager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>();
        // _cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();
        // _dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        // _questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        // _dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        // _inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
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
