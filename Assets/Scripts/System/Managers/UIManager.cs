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
    private GameObject choiceContainer;     //선택지 창
    
    [SerializeField]
    private NodeParser _nodeParser;         //노드 parser. 추후 어디에 병합할지 고려해야 한다.
    private Player player;                  //플레이어


    private void Start()
    {
        choiceContainer = dialogueContainer.transform.GetChild(1).gameObject;   //dialogueContainer의 두번째 자식: 선택 창. 
        player = GameObject.FindWithTag("Player").GetComponent<Player>();       //하나뿐인 Player을 찾아 스크립트 초기화
    }

    public void OpenInteractionButton() //상호작용 가능한 trigger에 들어올 시 상호작용 버튼을 띄운다.
    {
        interactionKey.DOFade(1, 0.5f);
        interactionKey.rectTransform.DOAnchorPosY
            (interactionKey.rectTransform.anchoredPosition.y + 30, 0.5f);   //아래 -> 위 slide

        interactionKey.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void CloseInteractionButton()
    {
        interactionKey.rectTransform.DOAnchorPosY
            (interactionKey.rectTransform.anchoredPosition.y - 30, 0.5f);
        interactionKey.DOFade(0, 0.5f);

        interactionKey.transform.GetChild(0).gameObject.SetActive(false);
    }

    /*
     * 이름: OpenDialoguePopup
     * 기능: interaction 시 대화창, 관련된 dialogue node 읽기 시작
     * 인자: X
    */
    public void OpenDialoguePopup(DialogueGraph graph)
    {
        dialogueContainer.SetActive(true);
        _nodeParser.NodeParseStart(graph);
        CloseInteractionButton();
    }
    public void CloseDialoguePopup()
    {
        choiceContainer.SetActive(false);
        dialogueContainer.SetActive(false);
        // player.isInteracting = false;
        CameraController.Instance.ReturnInteractionView();
    }

    public void OpenChoicePopup()
    {
        choiceContainer.SetActive(true);
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
