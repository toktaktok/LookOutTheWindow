using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;

public class MiniGameManager : Singleton<MiniGameManager>
{
    
    public List<MiniGame> miniGames;
    public List<GameObject> miniGames_gameObject;
    public Ease ease;
    public MiniGame currentMiniGame;

    [SerializeField] private float answerTurnTime;
    [SerializeField] private Transform miniGameCanvas;
    private int minigameId;
    
    private void Start()
    { 
        miniGames = new List<MiniGame>(); 
        // 특정 이름의 오브젝트를 찾아 넣을 때 오류 try-catch 필요한가?
        miniGameCanvas = GameObject.Find("MiniGameCanvas").GetComponent<Transform>();
        StartCoroutine(CalculateAnswerTime(10)); //답변 시간을 계산하는 코루틴 시작(테스트)
        AppendMiniGameList(miniGameCanvas);
    }

    private void AppendMiniGameList(Transform canvas)
    {
        miniGames_gameObject.Add(null);
        for (var i = 0; i < canvas.childCount; i++)
        {
            miniGames_gameObject.Add(canvas.GetChild(i).gameObject);
            miniGames.Add(miniGames_gameObject[i + 1].GetComponent<MiniGame>());
            miniGames_gameObject[i + 1].SetActive(false);
        }
        // Debug.Log("미니게임 추가 완료");
        miniGames[2].SetActiveTest();
    }

    // 이름: MiniGameStartSetting
    // 기능: 미니게임 시작 시 카메라, UI 맞춰 세팅. 함수 호출하며 id 전달
    // 인자: int gameId
    public void MiniGameStartSetting(int gameId)
    {
        UIManager.Instance.CloseInteractionKey();
        OpenMiniGameView(gameId);
    }

    IEnumerator CalculateAnswerTime(float answerTime)
    {
        float checkingTime = 0;
        var waitSec = new WaitForSeconds(0.1f);
        while (checkingTime < answerTime)
        {
            checkingTime += Time.fixedDeltaTime;
            // Debug.Log( "남은 시간: " + (answerTime - checkingTime));
            yield return null;
        }
        // Debug.Log("답변 대기 시간 끝");
    }

    public void EndAnswerTurn()
    {
        // 답변 턴 끝.
    }

    public void ChangeMiniGameState()
    {
        switch (currentMiniGame.curState)
        {
            case EnumTypes.CurrentMiniGameState.Good:
                break;
            case EnumTypes.CurrentMiniGameState.SoSo:
                break;
            case EnumTypes.CurrentMiniGameState.Bad:
                break;
            default:
                break;
        }
    }
    
    // 이름: OpenMiniGameView
    // 기능: 전달받은 id에 따라 미니게임 실행
    private void OpenMiniGameView(int id)
    {
        switch(id)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                miniGames[id].GetComponent<GameObject>().SetActive(true);
                // miniGames[id].SetActive(true);
                miniGames[id].GetComponent<RectTransform>().DOAnchorPosX(-480, 0.8f).SetEase(ease);
                minigameId = id;
                break;
        }
        OpenMiniGameCam();
    }
    private static void OpenMiniGameCam()
    {
        CameraController.Instance.mainCam.DORect(new Rect(0, 0, 0.5f, 1), 0.8f)
            .SetEase(Ease.OutQuart);
        CameraController.Instance.miniCam.DORect(new Rect(0.5f, 0, 0.5f, 1), 0.8f)
            .SetEase(Ease.OutQuart);
    }
    
    
    public void CloseMiniGameView() //미니게임 캔버스를 닫는다.
    {
        // if (miniGames[minigameId].activeSelf == true)

        if (miniGames[minigameId].GetComponent<GameObject>().activeSelf == true)
        {
            miniGames[minigameId].GetComponent<RectTransform>().DOAnchorPosX(480, 0.8f).SetEase(ease)
                .OnComplete(() => {
                    // miniGames[minigameId].SetActive(false); // 미니게임 UI를 화면 밖으로 밀고, 비활성화한다.

                    miniGames[minigameId].GetComponent<GameObject>().SetActive(false); // 미니게임 UI를 화면 밖으로 밀고, 비활성화한다.
                });
            minigameId = 0; // id를 0으로 초기화
        }
        CloseMiniGameCam();
    }
    private static void CloseMiniGameCam()
    {
        CameraController.Instance.mainCam.DORect(CameraController.Instance.mainOrigRect, 1f).SetEase(Ease.OutQuart);
        CameraController.Instance.miniCam.DORect(CameraController.Instance.miniOrigRect, 1f).SetEase(Ease.OutQuart);
    }

    public bool IsMiniGamePlaying() //미니게임이 실행 중인가?
    {
        if (miniGames == null)
        {
            return false;
        }

        foreach (var games in miniGames){
            if (games.GetComponent<GameObject>().activeSelf)
                return true;
        }

        return false;
    }


}
