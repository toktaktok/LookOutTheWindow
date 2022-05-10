using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : Singleton<GameInstance>
{
    //신을 전환해도 유지되는 객체들 선언
    private ScriptableObjects.GamePrefabs _gamePrefabs;

    public ScriptableObjects.GamePrefabs GamePrefabs
    {
        get { return _gamePrefabs; }
    }
}
