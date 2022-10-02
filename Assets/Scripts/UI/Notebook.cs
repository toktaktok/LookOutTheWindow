using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class Notebook : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject coverPage;
    [SerializeField] private GameObject questPage;

    private const float defaultClosedYPos = 1080;

    private void Start()
    {
        rectTransform.anchoredPosition = new Vector2(0, defaultClosedYPos);
    }

    public void TrueActiveSelf() => gameObject.SetActive(true);
    public void FalseActiveSelf() => gameObject.SetActive(false);

    public bool CheckActiveSelf()
    {
        return gameObject.activeSelf;
    }

    public void Open()
    {
        Debug.Log("수첩 열기");
        TrueActiveSelf();
        rectTransform.DOAnchorPosY(0, 0.6f).SetEase(Ease.InOutSine);
    }
    
    public void Close()
    {
        Debug.Log("수첩 닫기");
        rectTransform.DOAnchorPosY(defaultClosedYPos, 0.6f).SetEase(Ease.InSine);
        FalseActiveSelf();
    }

    public void MoveToQuestPage(int questId)
    {
        UpdateQuestPageText(questId);
    }

    private void UpdateQuestPageText(int id)
    {
        //의뢰 페이지로 이동할 때 정보 갱신
    }

}
