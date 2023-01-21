using System;
using System.Collections;
using Structs;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DG.Tweening;
using UnityEngine;



public class MapManager : Singleton<MapManager>
{
    public GameObject bigStreet_a;
    public GameObject bigStreet_b;
    public GameObject smallStreet_a;
    public GameObject ground;
    public List<List<Gate>> currentGateList = new List<List<Gate>>();
    
    public Dictionary<string, List<Gate>> curGateList = new Dictionary<string, List<Gate>>();
    private List<int> playerYPos;
    // private int currentPos = 0;
    private Player _player;
    private Vector3 groundsRelativePos;
    private float delayTime = 0.6f;
    private float op = 1;


    public void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerYPos = new List<int>{35, 30, 70}; //차례로 bigStreet a, subStreet a, bigStreet b의 y pos
        StartCoroutine(CurGateListCheck());
    }

    private IEnumerator CurGateListCheck()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var streetsName in curGateList.Keys)
        {
            Debug.Log(streetsName);
        }
    }

    public void MoveToAnotherStreet(Passage passage) //맵 이동
    {
        
        if (curGateList.TryGetValue(passage.street.name, out var curStreet)) //게이트 리스트를 가진 거리 검색
        {
            foreach (var gate in curStreet) //그 거리의 게이트들 모두 조정
            {
                switch (passage.moveType)
                {
                    case MoveType.Enter:
                        // passage.itsStreet.OffFront();
                        if (gate.leftBuilding.transform.GetChild(0).gameObject.TryGetComponent<Material>(out var mat)) //(gate.leftBuilding.transform.GetChild(0).gameObject.TryGetComponent<Material>(out var mat))
                        {
                            op = 0.2f;
                            //코루틴이나 tween 이용해서 투명도 애니메이션 만들기
                            mat.SetFloat("Opacity",  op);
                            // StartCoroutine(FadeOutBuilding(op));
                        }
                        gate.leftBuilding.transform.DOScale(1.2f, delayTime);
                        gate.leftBuilding.transform.DOLocalMove(new Vector3(gate.leftOrigPos.x - 3, 0, gate.leftOrigPos.z - 20), delayTime);
                        gate.rightBuilding.transform.DOScale(1.2f, delayTime);
                        gate.rightBuilding.transform.DOLocalMove(new Vector3(gate.rightOrigPos.x + 3, 0,  gate.rightOrigPos.z - 20), delayTime);
                        // passage.street.SetActive(false);
                        passage.exitText.SetActive(true);
                        break;
                    case MoveType.Exit:
                        // passage.street.SetActive(true);
                        gate.leftBuilding.transform.DOScale(1.0f, delayTime);
                        gate.leftBuilding.transform.DOLocalMove(new Vector3(gate.leftOrigPos.x, 0, gate.leftOrigPos.z), delayTime);
                        gate.rightBuilding.transform.DOScale(1.0f, delayTime);
                        gate.rightBuilding.transform.DOLocalMove(new Vector3(gate.rightOrigPos.x, 0,  gate.rightOrigPos.z), delayTime);
                        passage.exitText.SetActive(false);
                        break;
                    default:
                        break;
                }
                
            }
        }
        
        _player.MoveStreet(passage.exitPos.position);
        // MoveGround(passage.exitPos.position);
        // CameraController.Instance.mainCam.transform.DORotateQuaternion(Quaternion.Euler(-1, 0, 0), 0.2f);
        
    }

    //건물에 들어갈 시의 이동
    public void EnterBuilding(Transform pos)
    {
        _player.MoveToDestInstant(pos);
        CameraController.Instance.MoveCamInstant(pos.position);
        // CameraController.Instance.InsideCamera(pos.position, pos.rotation);
    }

    private IEnumerator FadeOutBuilding(float opacity)
    {
        while (opacity > 0)
        {
            opacity -= 0.1f;
            yield return null;
        }
    }

    public void MoveGround(Vector3 dest)
    {
        // ground.transform.DOMove(dest, 1f);
        // ground.transform.DOMoveZ(dest.z, 0.8f);
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


