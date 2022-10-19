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
    [SerializeField] private Animator noteAnim;
    [SerializeField] private GameObject stickyNote;

    private const float defaultClosedYPos = -1080;
    private const int defaultPageXPos = 250;
    readonly WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);


    private void Start()
    {
        rectTransform.anchoredPosition = new Vector2(0, defaultClosedYPos);
        FalseActiveSelf();
        questPage.SetActive(false);
        stickyNote.SetActive(false);
    }

    public void TrueActiveSelf() => gameObject.SetActive(true);
    public void FalseActiveSelf() => gameObject.SetActive(false);

    public bool CheckActiveSelf()
    {
        return gameObject.activeSelf;
    }

    public void Open()
    {
        TrueActiveSelf();
        stickyNote.SetActive(true);

        rectTransform.DOAnchorPosY(0, 0.6f).SetEase(Ease.OutQuart);
    }
    
    public IEnumerator Close()
    {
        rectTransform.DOAnchorPosX(0, 0.2f).SetEase(Ease.Linear);
        noteAnim.ResetTrigger("OpenCover");
        noteAnim.SetTrigger("CloseCover");
        yield return waitForSeconds;
        // questPage.SetActive(false);
        rectTransform.DOAnchorPosY(defaultClosedYPos, 0.4f).SetEase(Ease.InSine);
        yield return waitForSeconds;
        UIManager.Instance.ShowNoteBookButton();
        FalseActiveSelf();
    }

    public void MoveToQuestPage(int questId)
    {
        rectTransform.DOAnchorPosX(defaultPageXPos, 0.2f).SetEase(Ease.Linear);
        noteAnim.SetTrigger("OpenCover");
        stickyNote.SetActive(false);
        UpdateQuestPageText(questId);
        // questPage.SetActive(true);
    }

    private void UpdateQuestPageText(int id)
    {
        //의뢰 페이지로 이동할 때 정보 갱신
    }

}
