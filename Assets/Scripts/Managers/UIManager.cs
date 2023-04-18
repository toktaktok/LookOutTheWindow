using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EnumTypes;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;

// 대부분의 시스템 ui를 담당하는 UIManager.
// 수첩, 시스템 창, 대화 창 등의 팝업을 켜고 끄는 역할을 담당한다.
public class UIManager : Singleton<UIManager>
{

    public Ease ease;
    public GameObject dialogueContainer;    //대사 창
    public GameObject dialogueBubble;       //대사 말풍선

    public Image notebookButton;
    public Notebook notebook;
    public InteractionKey interactKey;
    public GameObject[] backGroundSprites;
    
    
    [SerializeField] private GameObject moveToUI;
    [SerializeField] private bool canOpenNotebook = true;

    private Canvas _uICanvas;
    private Canvas _miniGameCanvas;
    private UIState _curUIState = UIState.Basic;
    private Player _player;                                 //플레이어
    private GameObject _choiceContainer;                    //선택지 창
    private float _interactKeyOrigPosY;                     //상호작용 키 y 위치
    private float _noteButtonOrigPosY;                      //수첩 버튼 y 위치
    private float _noteOrigPosY;                            //수첩 y 위치
    private readonly WaitForSeconds _notebookDelayTime = new WaitForSeconds(0.7f);

    
    
    private void Start()
    {
        #region Init
        GameManager.Instance.curGameFlowState = GameFlowState.InGame;
        _curUIState = UIState.Basic;
        _choiceContainer = dialogueContainer.transform.GetChild(1).gameObject;   //dialogueContainer의 두번째 자식: 선택 창. 
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();       //하나뿐인 Player을 찾아 스크립트 초기화
        _noteButtonOrigPosY = notebookButton.rectTransform.anchoredPosition.y;
        interactKey.Set(); // 상호작용 키 UI 기본 세팅
        GameManager.Input.keyAction += OnKeyboard;
        _choiceContainer.SetActive(false);
        dialogueContainer.SetActive(false);
        #endregion
    }

    
    // 이름: OpenDialoguePopup
    // 기능: interaction 시 대화창을 열고, 관련된 dialogue node를 읽기 시작한다.
    public IEnumerator OpenDialoguePopup()
    {
        ChangeUIState(UIState.Interacting);
        _player.SwitchSpeed(true);
        CloseInteractionKey();
        HideNoteBookButton();
        yield return new WaitForSeconds(0.1f);
        dialogueContainer.SetActive(true); //대사 창 오픈
    }

    public IEnumerator OpenDialogueBubble()
    {
        ChangeUIState(UIState.Interacting);
        CloseInteractionKey();
        
        dialogueBubble.SetActive(true);
        yield return null;
    }

    public void CloseDialoguePopup()
    {
        GameManager.Instance.curGameFlowState = GameFlowState.InGame;
        ChangeUIState(UIState.Basic);
        _choiceContainer.SetActive(false);
        dialogueContainer.SetActive(false);
        _player.SwitchSpeed(false);
        _player.EraseInteractingObject();
        CameraManager.Instance.ReturnInteractionView();
        CharacterManager.Instance.StopTalk();
        ShowNotebookButton();

    }



    public void OpenChoicePopup()
    {
        _choiceContainer.SetActive(true);
        //현재 주민이 진행중인 퀘스트의 정보를 줄 수 있는지?
        QuestManager.Instance.CanTargetGiveEvidence();
    }

    public void CloseChoicePopup() => _choiceContainer.SetActive(false);

    //기능: 마우스 포인터가 수첩 버튼에 가까이 갈 때 UI를 올린다.
    public void MouseEnterNotebookButton()
    {
        if (_curUIState != UIState.Basic)
        {
            return;
        }
        notebookButton.rectTransform.DOAnchorPosY(_noteButtonOrigPosY + 50, 0.2f);
    }
 
    public void ShowNotebookButton() => notebookButton.rectTransform.DOAnchorPosY(_noteButtonOrigPosY, 0.3f);

    //TODO: 마우스를 노트북 위치에 대고 열 때 다시 노트북 버튼 등장하지 않도록
    public void OpenNotebook()
    {
        ChangeUIState(UIState.NoteBook);
        HideNoteBookButton();
        notebook.TrueActiveSelf();
        notebook.Open();
    }
    public void CloseNotebook()
    {
        notebook.StartCoroutine(notebook.Close());
        ChangeUIState(UIState.Basic);
        ShowNotebookButton();
        notebook.FalseActiveSelf();
    }

    private IEnumerator NotebookTimer()
    {
        canOpenNotebook = false;
        yield return _notebookDelayTime;
        canOpenNotebook = true;
    }

    private void OnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuestManager.Instance.ChangeQuestToRequestingState();
        }
        
        if (Input.GetKeyDown(KeyCode.I) && canOpenNotebook)
        {

            switch (_curUIState)
            {
                case UIState.Interacting:
                    break;
                case UIState.Basic:
                    OpenNotebook();
                    break;
                case UIState.NoteBook:
                    CloseNotebook();
                    break;
            }

            StartCoroutine(NotebookTimer());
        }
    }

    public void ChangeUIState(UIState state) => _curUIState = state;
    
    public void OpenInteractionKey()  => interactKey.Open();
    public void CloseInteractionKey() => interactKey.Close();
    
    public void MouseExitNotebookButton() => notebookButton.rectTransform.DOAnchorPosY(_noteButtonOrigPosY, 0.2f);
    public void HideNoteBookButton() => notebookButton.rectTransform.DOAnchorPosY(_noteButtonOrigPosY - 200, 0.3f);

    public void OpenMapMovingUI(Passage passage) =>  moveToUI.SetActive(true);
    public void CloseMapMovingUI() =>  moveToUI.SetActive(false);
}
