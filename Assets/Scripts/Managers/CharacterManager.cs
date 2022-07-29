using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    //캐릭터에 저장된 정보를 관리하는 매니저. -> 현재 필요한가?
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
