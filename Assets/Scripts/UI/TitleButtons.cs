using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtons : MonoBehaviour
{
    public void OnNewGame()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void OnLoadGame()
    {
        //이후 저장된 정보 로드할 수 있도록
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void OnSetting()
    {
        
    }
}
