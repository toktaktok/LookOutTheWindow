using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;

public class Item : MonoBehaviour
{
    public Interactable interactable;

    public void Check()
    {
        DialogueManager.Instance.ParseStart(interactable.DialogueGraphs[0]);

        Debug.Log("오브젝트 id : " + interactable.Id);
        Debug.Log("오브젝트 이름 : " + interactable.Name);
        Debug.Log("Minigame Id : " + interactable.Minigameid);
        interactable.CheckMinigame();
    }
}
