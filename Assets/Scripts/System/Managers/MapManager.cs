using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject bigStreet_a;
    public GameObject bigStreet_b;
    public GameObject smallStreet_a;

    public List<int> playerYPos;
    public int currentPos = 0;

    public void Start()
    {
        playerYPos = new List<int>{10, 30, 60};
    }
    public void OpenMapChunk(GameObject chunk)
    {
        chunk.SetActive(true);
    }
    
    public void CloseMapChunk(GameObject chunk)
    {
        chunk.SetActive(false);
    }
}

public class Map
{
    
}
