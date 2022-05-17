using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : Singleton<MinigameManager>
{


    public void StartSetting(int gameId)
    {
        CameraController.Instance.MakeMinigameView();
        UIManager.Instance.CloseInteractionUI();
        UIManager.Instance.OpenMinigameView(gameId);
    }


}
