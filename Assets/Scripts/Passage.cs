using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject exitText;
    public Street itsStreet;

    private void Start()
    {

        StartCoroutine(Init());

    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(0.1f);
        
        if (moveType == MoveType.Exit)
        {
            exitText = transform.GetChild(0).gameObject; //출구 텍스트를 출구 포탈의 첫번째 차일드에서 받아옴
            exitText.SetActive(false);
        }
        
    }
}
