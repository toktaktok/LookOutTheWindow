using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private GameManager _game;
    // private List<Villager> _villagers = new List<Villager>();


    public GameManager GameManager
    {
        get { return _game; }
    }

    public void Init(GameManager game)
    {
        _game = game;
    }
}
