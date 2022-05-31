using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    public Interactable interactable;
    
    //CharacterManager의 하위 객체. CharacterManager을 역참조한다
    protected CharacterManager _manager;

    public void Init(CharacterManager manager)
    {
        _manager = manager;
    }
    
    public void Check()
    {
        UIManager.Instance.OpenInteractionUI();
        Debug.Log("주민 id : " + interactable.Id);
        Debug.Log("주민 이름 : " + interactable.Name);
        // Debug.Log("Minigame Id : " + interactable.Minigameid);
        interactable.CheckMinigame();
    }

}
