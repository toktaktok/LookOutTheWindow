using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class DragDrop: MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    Canvas canvas;

    RectTransform rectTransform;
    public float FloorPos;
    int fallingSpeed = 20;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        FloorPos = rectTransform.anchoredPosition.y;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float posDifference = rectTransform.anchoredPosition.y - FloorPos;
        ObjectFalling(posDifference);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("쓰레기 정리!");
        gameObject.SetActive(false);
    }

    void ObjectFalling(float difference) //
    {

        rectTransform.DOAnchorPosY(FloorPos, 0.5f).SetEase(Ease.InSine).OnComplete(() =>
        {
            rectTransform.DOAnchorPosY
            (rectTransform.anchoredPosition.y + (difference * 0.2f), 0.25f).SetEase(Ease.OutSine)
            .OnComplete(() =>
           {
               rectTransform.DOAnchorPosY
               (FloorPos, 0.25f).SetEase(Ease.InSine);
           });
        });
        

    }


}
