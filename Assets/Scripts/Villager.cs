using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    //CharacterManager의 하위 객체. CharacterManager을 역참조한다
    protected CharacterManager _manager;

    public void Init(CharacterManager manager)
    {
        _manager = manager;
    }

}
