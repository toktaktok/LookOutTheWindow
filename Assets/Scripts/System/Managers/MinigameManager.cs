using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;

public class MinigameManager : Singleton<MinigameManager>
{
    public GameObject[] minigames;
    int minigameId;
    public Ease ease;


    public void StartSetting(int gameId)
    {
        
        CameraController.Instance.MakeMinigameView();
        UIManager.Instance.CloseInteractionButton();
        OpenMinigameView(gameId);
    }
    
    public void OpenMinigameView(int id)
    {
        switch(id)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                minigames[id].SetActive(true);
                minigames[id].GetComponent<RectTransform>().DOAnchorPosX(-480, 0.8f).SetEase(ease);
                minigameId = id;
                break;
        }
    }
    
    public void CloseMinigameView() //미니게임 캔버스를 닫는다.
    {
        if (minigames[minigameId].activeSelf == true)
        {
            minigames[minigameId].GetComponent<RectTransform>().DOAnchorPosX(480, 0.8f).SetEase(ease)
                .OnComplete(() => {
                    minigames[minigameId].SetActive(false);
                });
            minigameId = 0;
        }
    }

    public bool IsMinigamePlaying()
    {
        if (minigames != null)
        {
            foreach (var games in minigames){
                if (games.activeSelf)
                    return true;
            }
        }
        return false;
    }


}
