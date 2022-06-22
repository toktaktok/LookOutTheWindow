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

    
    /*
     * 이름: MinigameStartSetting
     * 기능: 미니게임 시작 시 카메라, UI 맞춰 세팅. 함수 호출하며 id 전달
     * 인자: int gameId
    */
    public void MinigameStartSetting(int gameId)
    {
        UIManager.Instance.CloseInteractionButton();
        OpenMinigameView(gameId);
    }
    
    
    /*
     * 이름: OpenMinigameView
     * 기능: 전달받은 id에 따라 미니게임 실행
     * 인자: int id
    */
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
        OpenMinigameCam();
    }
    private void OpenMinigameCam()
    {
        CameraController.Instance.main.DORect(new Rect(0, 0, 0.5f, 1), 0.8f)
            .SetEase(Ease.OutQuart);
        CameraController.Instance.mini.DORect(new Rect(0.5f, 0, 0.5f, 1), 0.8f)
            .SetEase(Ease.OutQuart);
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
        CloseMinigameCam();
    }
    private void CloseMinigameCam()
    {
        CameraController.Instance.main.DORect(CameraController.Instance.mainOrigRect, 1f).SetEase(Ease.OutQuart);
        CameraController.Instance.mini.DORect(CameraController.Instance.miniOrigRect, 1f).SetEase(Ease.OutQuart);
        
    }

    public bool IsMinigamePlaying() //미니게임이 실행 중인가?
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
