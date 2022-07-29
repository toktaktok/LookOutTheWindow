using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    Exit,
    Enter
}

public class Passage : MonoBehaviour
{
    public MoveType moveType;
    public Transform exitPos;
    public GameObject street;
}
