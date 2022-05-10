using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interactable", menuName = "ScriptableObjects/Interactable", order = 1)]
public class Interactable : ScriptableObject
{
    [SerializeField]
    private Sprite _image;

    [SerializeField]
    private string _id; //고유 번호 

    [SerializeField]
    private int _minigameId = 0; //미니게임 번호

    [SerializeField]
    private string _name;

    #region Foundation
    public Sprite Image
    {
        get { return _image; }
    }

    public string Id
    {
        get { return _id; }
    }

    public string Name
    {
        get { return _name; }
    }

    public int Minigameid
    {
        get { return _minigameId; }
    }

    #endregion

    public void CheckMinigame()
    {
        if (0 < Minigameid)
        {
            Debug.Log("미니게임 가능");
            MinigameManager.Instance.StartSetting(Minigameid);
        }
    }

}
