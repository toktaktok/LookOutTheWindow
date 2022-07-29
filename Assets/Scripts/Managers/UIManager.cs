using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;

public class UIManager : Singleton<UIManager>
{
    /* 대부분의 시스템 ui를 담당하는 UIManager.
     * 수첩, 시스템 창, 대화 창 등의 팝업을 켜고 끄는 역할을 담당한다.
     */
    public Image interactionKey;        //Interactable Object에 접근했을 때 뜨는 interaction ui.
    public Ease ease;       //ease 값
    public GameObject dialogueContainer;    //대사 창
    public Image notebookButton;
    public GameObject notebook;
    
    
    [SerializeField] private GameObject moveToUI;
    private Player player;                  //플레이어
    private GameObject choiceContainer;     //선택지 창
    private float interactKeyOrigPosY;      //상호작용 키 y 위치
    private float noteButtonOrigPosY;       //수첩 버튼 y 위치





    private void Start()
    {
        choiceContainer = dialogueContainer.transform.GetChild(1).gameObject;   //dialogueContainer의 두번째 자식: 선택 창. 
        player = GameObject.FindWithTag("Player").GetComponent<Player>();       //하나뿐인 Player을 찾아 스크립트 초기화
        interactKeyOrigPosY = interactionKey.rectTransform.anchoredPosition.y;
        noteButtonOrigPosY = notebookButton.rectTransform.anchoredPosition.y;
    }

    public void OpenInteractionKey() //상호작용 가능한 trigger에 들어올 시 상호작용 버튼을 띄운다.
    {
        interactionKey.rectTransform.DOAnchorPosY(interactKeyOrigPosY + 30, 0.5f);   //아래 -> 위 slide
        interactionKey.DOFade(1, 0.5f);
        interactionKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(1, 0.5f);
    }
    public void CloseInteractionKey()
    {
        interactionKey.rectTransform.DOAnchorPosY(interactKeyOrigPosY, 0.5f);   //위 -> 아래 slide
        interactionKey.DOFade(0, 0.5f);
        interactionKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 0.5f);
    }

    /*
     * 이름: OpenDialoguePopup
     * 기능: interaction 시 대화창, 관련된 dialogue node 읽기 시작
     * 인자: DialogueGraph
    */
    public void OpenDialoguePopup()
    {
        dialogueContainer.SetActive(true);
        CloseInteractionKey();
    }
    
    public void CloseDialoguePopup()
    {
        choiceContainer.SetActive(false);
        dialogueContainer.SetActive(false);
        // player.isInteracting = false;
        CameraController.Instance.ReturnInteractionView();
    }

    public void OpenMapMovingUI(Passage passage)
    {
        moveToUI.SetActive(true);
    }
    public void CloseMapMovingUI()
    {
        moveToUI.SetActive(false);
    }

    public void OpenChoicePopup()
    {
        choiceContainer.SetActive(true);
    }

    //수첩 버튼에 마우스 포인터가 올라갈 시 호출
    public void MouseEnterNotebookButton()
    {
        notebookButton.rectTransform.DOAnchorPosY(noteButtonOrigPosY + 50, 0.2f);
    }

    public void MouseExitNotebookButton()
    {
        notebookButton.rectTransform.DOAnchorPosY(noteButtonOrigPosY, 0.2f);
    }

    public void OpenNotebook()
    {
        notebookButton.rectTransform.DOAnchorPosY(noteButtonOrigPosY - 60, 0.2f);
        notebook.SetActive(true);

    }

    public void CloseNotebook()
    {
        notebookButton.rectTransform.DOAnchorPosY(noteButtonOrigPosY, 0.2f);
        notebook.SetActive(false);


    }

    // public void CloseMinigameView() //미니게임 캔버스를 닫는다.
    // {
    //     if (minigames[minigameId].activeSelf == true)
    //     {
    //         minigames[minigameId].GetComponent<RectTransform>().DOAnchorPosX(480, 0.8f).SetEase(ease)
    //             .OnComplete(() => {
    //                 minigames[minigameId].SetActive(false);
    //             });
    //         minigameId = 0;
    //     }
    //
    // }

}
