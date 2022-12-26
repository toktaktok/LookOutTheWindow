using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Street : MonoBehaviour
{
    [SerializeField] private List<Street> front;
    private List<Street> back;
    [SerializeField] private GameObject itsGameObject;
    [SerializeField] private List<Gate>itsGateList = new List<Gate>();

    private void Start()
    {
        itsGameObject = gameObject;
        if (itsGateList.Count > 0)
        {
            foreach (var curGate in itsGateList)
            {
                curGate.leftOrigPos = curGate.leftBuilding.localPosition;
                curGate.rightOrigPos = curGate.rightBuilding.localPosition;
            }
            MapManager.Instance.curGateList.Add(name, itsGateList);
        }
        
    }

    public void OffFront()
    {
        foreach (var street in front)
        {
            street.FalseActiveSelf();
        }
    }
    
    public void TrueActiveSelf() => gameObject.SetActive(true);
    private void FalseActiveSelf() => gameObject.SetActive(false);
}

//포탈 양 옆 빌딩 구조체
[System.Serializable]
public class Gate
{
    public Transform leftBuilding;
    public Transform rightBuilding;
    public Vector3 leftOrigPos;
    public Vector3 rightOrigPos;
}