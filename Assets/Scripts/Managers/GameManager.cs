using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnumTypes;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public GameFlowState curGameFlowState = GameFlowState.InGame;
    public bool isInteracted;
    
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
        if (FindObjectOfType<CameraManager>())
        {
            cameraManager = FindObjectOfType<CameraManager>().GetComponent<CameraManager>();
        }
        if (FindObjectOfType<DataManager>())
        {
            _dataManager = FindObjectOfType<InputManager>().GetComponent<DataManager>();
        }
        if (FindObjectOfType<UIManager>())
        {
            _uIManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
        }
        if (FindObjectOfType<DataManager>())
        {
            _dataManager = FindObjectOfType<DataManager>().GetComponent<DataManager>();
        }
        if (FindObjectOfType<DialogueManager>())
        {
            _dialogueManager = FindObjectOfType<DialogueManager>().GetComponent<DialogueManager>();
        }
        if (FindObjectOfType<CharacterManager>())
        {
            _characterManager = FindObjectOfType<CharacterManager>().GetComponent<CharacterManager>();
        }
        if (FindObjectOfType<QuestManager>())
        {
            _questManager = FindObjectOfType<QuestManager>().GetComponent<QuestManager>();
        }
        if (FindObjectOfType<MiniGameManager>())
        {
            _miniGameManager = FindObjectOfType<MiniGameManager>().GetComponent<MiniGameManager>();
        }
        if (FindObjectOfType<MapManager>())
        {
            _mapManager = FindObjectOfType<MapManager>().GetComponent<MapManager>();
        }

    }

    private void Update()
    {
        // _inputManager.OnUpdate();
        Input.OnUpdate();
    }

    public void UpdateQuestState()
    {
        foreach (var pair in _questManager.RequestingVillagerList)
        {
            Villager villager = _characterManager.SearchVillager(pair.Value.requestedVillager);
            Debug.Log(villager.Name);
            villager.SetupRequestingState();
        }
    }

    private void OnKeyboard()
    {

    }

    
    #region 매니저 변수 선언
    
    [SerializeField] private InputManager _inputManager;
    
    [SerializeField] private CameraManager cameraManager;
    
    [SerializeField] private UIManager _uIManager;
    
    [SerializeField] private DataManager _dataManager;
    
    [SerializeField] private CharacterManager _characterManager;
    
    [SerializeField] private MiniGameManager _miniGameManager;

    [SerializeField] private DialogueManager _dialogueManager;

    [SerializeField] private QuestManager _questManager;

    [SerializeField] private MapManager _mapManager;

    #endregion

    public static InputManager Input
    {
        get => Instance._inputManager;
    }
    
}
