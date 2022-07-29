using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    
}
public class Street
{
    private List<Street> front;
    private List<Street> back;
}

public class MapManager : Singleton<MapManager>
{
    public GameObject bigStreet_a;
    public GameObject bigStreet_b;
    public GameObject smallStreet_a;

    private List<int> playerYPos;
    // private int currentPos = 0;
    private Player _player;

    public void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerYPos = new List<int>{10, 30, 60}; //차례로 bigStreet a, subStreet a, bigStreet b의 y pos
    }

    public void MoveToAnotherStreet(Passage passage)
    {
        _player.MoveStreet(passage.exitPos.position.z);
        
        switch (passage.moveType)
        {
            case MoveType.Enter:
                passage.street.SetActive(false);
                break;
            case MoveType.Exit:
                passage.street.SetActive(true);
                break;
           default:
               break;
        }
    }
    
    // public void OpenMapChunk(GameObject chunk)
    // {
    //     chunk.SetActive(true);
    // }
    //
    // public void CloseMapChunk(GameObject chunk)
    // {
    //     chunk.SetActive(false);
    // }
}


