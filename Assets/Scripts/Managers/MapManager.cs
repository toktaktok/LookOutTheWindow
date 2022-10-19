using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    public GameObject ground;

    private List<int> playerYPos;
    // private int currentPos = 0;
    private Player _player;
    private Vector3 groundsRelativePos;

    public void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerYPos = new List<int>{10, 30, 60}; //차례로 bigStreet a, subStreet a, bigStreet b의 y pos

    }

    public void MoveToAnotherStreet(Passage passage)
    {
        MoveGround(passage.exitPos.position);
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

    public void MoveGround(Vector3 dest)
    {
        // ground.transform.DOMove(dest, 1f);
        ground.transform.DOMoveZ(dest.z, 1f);
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


