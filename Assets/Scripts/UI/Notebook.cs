using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Structs;
using TMPro;
using Unity.VisualScripting;

public class Notebook : MonoBehaviour
{
    
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject coverPage;
    
    [SerializeField] private GameObject questPage;
    [SerializeField] private TextMeshProUGUI questTitle;
    private TextMeshProUGUI[] goals;
    [SerializeField] private TextMeshProUGUI goal1;
    [SerializeField] private TextMeshProUGUI goal2;
    [SerializeField] private TextMeshProUGUI goal3;
    [SerializeField] private TextMeshProUGUI goal4;
    [SerializeField] private TextMeshProUGUI goal5;
    [SerializeField] private Button reasonButton;
    [SerializeField] private TextMeshProUGUI exceptionText;
    [SerializeField] private TextMeshProUGUI rQuestA;
    [SerializeField] private TextMeshProUGUI rQuestB;
    [SerializeField] private Evidence _evidenceA;
    [SerializeField] private Evidence _evidenceB;
    [SerializeField] private Evidence _evidenceC;
    [SerializeField] private Animator noteAnim;
    [SerializeField] private GameObject stickyNote;

    private string curPage = "";
    private const float DefaultClosedYPos = -1080;
    private const int DefaultPageXPos = 250;
    private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(0.5f);


    private void Start()
    {
        rectTransform.anchoredPosition = new Vector2(0, DefaultClosedYPos);
        FalseActiveSelf();
        questPage.SetActive(false);
        stickyNote.SetActive(false);
        goals = new []{goal1, goal2, goal3, goal4, goal5};

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
        curPage = "main";
        rectTransform.DOAnchorPosY(0, 0.6f).SetEase(Ease.OutQuart);
    }
    
    public IEnumerator Close() //책 닫기.
    {
        if (curPage != "main")
        {
            rectTransform.DOAnchorPosX(0, 0.2f).SetEase(Ease.Linear); //가로 이동.
            noteAnim.ResetTrigger("OpenCover");
            noteAnim.SetTrigger("CloseCover");
            yield return _waitForSeconds;
        }

        // questPage.SetActive(false);
        rectTransform.DOAnchorPosY(DefaultClosedYPos, 0.4f).SetEase(Ease.InSine); // 화면 밖으로 나가기
        yield return _waitForSeconds;
        UIManager.Instance.ShowNotebookButton();
        FalseActiveSelf();
    }

    public void MoveToQuestPage(string questId)
    {
        curPage = "quest";
        rectTransform.DOAnchorPosX(DefaultPageXPos, 0.2f).SetEase(Ease.Linear);
        noteAnim.SetTrigger("OpenCover");
        stickyNote.SetActive(false);
        UpdateQuestPageText(questId);
        // questPage.SetActive(true);
    }

    private void UpdateQuestPageText(string id)
    {
        //의뢰 페이지로 이동할 때 정보 갱신
        var quest = QuestManager.Instance.questsInProgress[id];
        
        questTitle.text = quest.Title;
        
        var index = 0;
        foreach (var text in quest.TodoList)
        {
            goals[index].text = text;
            index++;
        }
        
        for(var i=4; i >= index; i--)
        {
            goals[i].text = "";
        }
        
    }
}
