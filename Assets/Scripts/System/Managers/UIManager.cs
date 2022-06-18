using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public Image interactionKey;
    public Ease ease;
    public GameObject dialogueContainer;
    private GameObject choiceContainer;
    
    [SerializeField]
    private NodeParser _nodeParser;
    private Player player;


    private void Start()
    {
        choiceContainer = dialogueContainer.transform.GetChild(1).gameObject;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void OpenInteractionButton() //상호작용 가능한 trigger에 들어올 시 상호작용 버튼을 띄운다.
    {
        interactionKey.DOFade(1, 0.5f);
        interactionKey.rectTransform.DOAnchorPosY
            (interactionKey.rectTransform.anchoredPosition.y + 30, 0.5f);

        interactionKey.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OpenDialoguePopup()
    {
        dialogueContainer.SetActive(true);
        _nodeParser.NodeParseStart();
        CloseInteractionButton();
    }

    public void CloseDialoguePopup()
    {
        choiceContainer.SetActive(false);
        dialogueContainer.SetActive(false);
        player.isInteracting = false;
    }

    public void OpenChoicePopup()
    {
        choiceContainer.SetActive(true);
    }

    public void CloseInteractionButton()
    {
        interactionKey.rectTransform.DOAnchorPosY
        (interactionKey.rectTransform.anchoredPosition.y - 30, 0.5f);
        interactionKey.DOFade(0, 0.5f);

        interactionKey.transform.GetChild(0).gameObject.SetActive(false);
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
