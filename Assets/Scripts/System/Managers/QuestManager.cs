using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{

    private GameManager _game;

    public GameManager GameManager
    {
        get { return _game; }
    }

    public void Init(GameManager game)
    {
        _game = game;
    }

    public Quest quest;

    public Player player;

    public GameObject questWindow;
    public Text dialogue;



}

