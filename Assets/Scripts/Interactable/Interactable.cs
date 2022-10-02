using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;
using XNode;

[CreateAssetMenu(fileName = "Interactable", menuName = "ScriptableObjects/Interactable", order = 1)]
public class Interactable : ScriptableObject
{
    [SerializeField]
    private Sprite _image;

    // [SerializeField]
    // private string _id; //고유 번호 

    [SerializeField]
    private int _minigameId = 0; //미니게임 번호

    [SerializeField]
    private string _name; //고유 이름

    [SerializeField]
    private DialogueGraph[] _dialogueGraphs;
    
    [SerializeField] int[] _basicDialogueId;

    [SerializeField] private InteractableData interactableInfo;
    
    
    #region Foundation
    public Sprite Image
    {
        get { return _image; }
    }

    // public string Id
    // {
    //     get { return _id; }
    // }

    public string Name
    {
        get { return _name; }
    }

    public int Minigameid
    {
        get { return _minigameId; }
    }
    

    public DialogueGraph[] DialogueGraphs
    {
        get { return _dialogueGraphs; }
    }

    #endregion

    
    public void GetEnumName()
    {
        // const bool ignoreCase = true;
        Debug.Log(Enum.GetName(typeof(InteractableData), interactableInfo)); 
        // Debug.Log(Enum.TryParse(Name, ignoreCase, out interactableInfo));
    }


    /*
     * 기능 : 오브젝트에 존재하는 미니게임 아이디를 체크한다.
     * 미니게임 아이디에 따라 미니게임매니저에서 함수를 호출한다.
     */
    public void CheckMiniGame()
    {
        if (0 == Minigameid)
        {
            return;
        }

        Debug.Log("미니게임 가능");
            MiniGameManager.Instance.MiniGameStartSetting(Minigameid);
        
    }
}
