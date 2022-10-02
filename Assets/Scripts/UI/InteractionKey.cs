using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class InteractionKey : MonoBehaviour
{
    [SerializeField] private Image itsImage;
    [SerializeField] private TextMeshProUGUI itsTextUI;

    private float interactKeyOrigPosY;      //상호작용 키 y 위치
    private RectTransform itsRectTransform;
    
    public void Set()
    {
        itsRectTransform = gameObject.GetComponent<RectTransform>();
        interactKeyOrigPosY = itsRectTransform.anchoredPosition.y;
    }
    //상호작용 가능한 trigger에 들어올 시 상호작용 버튼을 띄운다.
    public void Open() 
    {
        itsRectTransform.DOAnchorPosY(interactKeyOrigPosY + 30, 0.5f);   //아래 -> 위 slide
        itsImage.DOFade(1, 0.5f);
        itsTextUI.DOFade(1, 0.5f);
    }
    public void Close()
    {
        itsRectTransform.DOAnchorPosY(interactKeyOrigPosY, 0.5f);   //위 -> 아래 slide
        itsImage.DOFade(0, 0.5f);
        itsTextUI.DOFade(0, 0.5f);
    }
}
