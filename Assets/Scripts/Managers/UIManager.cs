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

    // public Image interactionKey;        //Interactable Object에 접근했을 때 뜨는 interaction ui.
    public Ease ease;       //ease 값
    public GameObject dialogueContainer;    //대사 창
    public Image notebookButton;
    public Notebook notebook;
    public InteractionKey interactKey;
    
    
    [SerializeField] private GameObject moveToUI;
    // [SerializeField] private GameObject interactUI;
    private Player player;                  //플레이어
    private GameObject choiceContainer;     //선택지 창
    private float interactKeyOrigPosY;      //상호작용 키 y 위치
    private float noteButtonOrigPosY;       //수첩 버튼 y 위치
    private float noteOrigPosY;       //수첩 y 위치
    [SerializeField] private bool canOpenNotebook = true;
    private readonly WaitForSeconds notebookDelayTime = new WaitForSeconds(0.7f);
    private Canvas UICanvas;
    private Canvas MinigameCanvas;
    private UIState curUIState = UIState.Basic;
    
    
    private void Start()
    {
        
        #region Init
        GameManager.Instance.curGameFlowState = GameFlowState.InGame;
        curUIState = UIState.Basic;
        choiceContainer = dialogueContainer.transform.GetChild(1).gameObject;   //dialogueContainer의 두번째 자식: 선택 창. 
        player = GameObject.FindWithTag("Player").GetComponent<Player>();       //하나뿐인 Player을 찾아 스크립트 초기화
        // interactKeyOrigPosY = interactionKey.rectTransform.anchoredPosition.y;
        noteButtonOrigPosY = notebookButton.rectTransform.anchoredPosition.y;
        interactKey.Set(); // 상호작용 키 UI 기본 세팅
        GameManager.Input.keyAction += OnKeyboard;
        choiceContainer.SetActive(false);
        dialogueContainer.SetActive(false);
        #endregion
    }

    
    // 이름: OpenDialoguePopup
    // 기능: interaction 시 대화창을 열고, 관련된 dialogue node를 읽기 시작한다.
    public IEnumerator OpenDialoguePopup()
    {
        curUIState = UIState.Interacting;
        player.SwitchSpeed(true);
        CloseInteractionKey();
        HideNoteBookButton();
        yield return new WaitForSeconds(0.1f);
        dialogueContainer.SetActive(true); //대사 창 오픈
        
    }

    public void CloseDialoguePopup()
    {
        GameManager.Instance.curGameFlowState = GameFlowState.InGame;
        curUIState = UIState.Basic;
        choiceContainer.SetActive(false);
        dialogueContainer.SetActive(false);
        player.SwitchSpeed(false);
        player.EraseInteractingObject();
        CameraController.Instance.ReturnInteractionView();
        CharacterManager.Instance.StopTalk();
        ShowNoteBookButton();

    }
    //상호작용 가능한 trigger에 들어올 시 상호작용 버튼을 띄운다.
    public void OpenInteractionKey()  => interactKey.Open();
    public void CloseInteractionKey() => interactKey.Close();

    public void OpenMapMovingUI(Passage passage) =>  moveToUI.SetActive(true);
    public void CloseMapMovingUI() =>  moveToUI.SetActive(false);

    public void OpenChoicePopup()
    {
        choiceContainer.SetActive(true);
        //현재 주민이 진행중인 퀘스트의 정보를 줄 수 있는지?
        QuestManager.Instance.CanTargetGiveEvidence();
    }

    public void CloseChoicePopup() => choiceContainer.SetActive(false);

    //기능: 마우스 포인터가 수첩 버튼에 가까이 갈 때 UI를 올린다.
    public void MouseEnterNotebookButton()
    {
        if (curUIState != UIState.Basic)
        {
            return;
        }
        notebookButton.rectTransform.DOAnchorPosY(noteButtonOrigPosY + 50, 0.2f);
    }
    public void MouseExitNotebookButton()
    {
        notebookButton.rectTransform.DOAnchorPosY(noteButtonOrigPosY, 0.2f);
    }

    public void HideNoteBookButton()
    {
        notebookButton.rectTransform.DOAnchorPosY(noteButtonOrigPosY - 200, 0.3f);
    }

    public void ShowNoteBookButton()
    {

        notebookButton.rectTransform.DOAnchorPosY(noteButtonOrigPosY, 0.3f);

    }
    //기능: 수첩 UI를 연다.
    public void OpenNotebook()
    {
        curUIState = UIState.NoteBook;
        HideNoteBookButton();
        notebook.TrueActiveSelf();
        notebook.Open();

    }
    public void CloseNotebook()
    {
        // if (curUIState != UIState.NoteBook)
        // {
        //     return;
        // }
        notebook.StartCoroutine(notebook.Close());
        curUIState = UIState.Basic;
        // ShowNoteBookButton();
        // notebook.FalseActiveSelf();
    }

    private IEnumerator NotebookTimer()
    {
        canOpenNotebook = false;
        yield return notebookDelayTime;
        canOpenNotebook = true;
    }

    private void OnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.I) && canOpenNotebook)
        {

            switch (curUIState)
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



}
