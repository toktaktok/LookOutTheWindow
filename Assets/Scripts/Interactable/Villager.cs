using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class Villager : MonoBehaviour
{
    public Interactable interactable;
    
    //CharacterManager의 하위 객체. CharacterManager을 역참조한다
    private CharacterManager _manager;

    public void Init(CharacterManager manager)
    {
        _manager = manager;
    }
    
    public void Check()
    {
        UIManager.Instance.OpenDialoguePopup(interactable.DialogueGraphs[0]);
        Debug.Log("주민 id : " + interactable.Id);
        Debug.Log("주민 이름 : " + interactable.Name);
        Debug.Log("minigame Id : " + interactable.Minigameid);
        interactable.CheckMinigame();
    }

}
