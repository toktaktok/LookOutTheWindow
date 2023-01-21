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

        Debug.Log("오브젝트 이름 : " + interactable.Name);
        Debug.Log("MiniGame Id : " + interactable.Minigameid);
        interactable.CheckMiniGame();
    }
}
