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
