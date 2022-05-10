using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    protected MinigameManager _manager;

    public void Init(MinigameManager manager)
    {
        _manager = manager;
    }

    public GameObject[] UIs;

    public int id;


    public void Start()
    {

    }

}
