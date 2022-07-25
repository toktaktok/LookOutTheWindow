using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class CameraController : Singleton<CameraController>
{

    public Transform target; //타켓으로 할 대상(플레이어 기본)

    [Range(0, 1)]
    public float zoomRange;         //카메라 줌 범위를 조절하는 변수
    private float savedZoomRange;    //기존 줌 범위
    // [Range(30, 60)]
    // public float fovSize;           //zoomRange에 따라 fieldOfView를 바꾸는 변수
    
    [Range(8, 40)]
    public float viewSize = 20;           //zoomRange에 따라 fieldOfView를 바꾸는 변수
    
    [Range(-25, 0)]
    float zoomXrot;                 //zoomRange에 따라 rotation.x를 조절하는 변수
    
    public float rotateValue;       //회전 값
    
    [SerializeField]
    private Ease defaultEase = Ease.OutSine;

    [SerializeField]
    private bool isCutscene = false;
 

    public Transform mainCamAnchor; //카메라의 회전축

    public Camera mainCam; //메인 카메라 
    public Camera miniCam; //미니게임용 카메라

    [HideInInspector]
    public Rect mainOrigRect;
    [HideInInspector]
    public Rect miniOrigRect;


    private void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();

        zoomXrot = -23;
        mainOrigRect = mainCam.rect;
        miniOrigRect = miniCam.rect;
        DOTween.SetTweensCapacity(2000, 50);
    }

    public void Zoom(InputAction.CallbackContext ctx)
    {
        zoomRange -= ctx.ReadValue<float>() * 0.005f;
        zoomRange = zoomRange switch
        {
            > 1 => 1,
            < 0 => 0,
            _ => rotateValue
        };
    }

    public void Rotate(InputAction.CallbackContext ctx)
    {
        if ( !ctx.performed ) { return; }   //callback 값 중 중간 값만 받는다
        
        rotateValue += ctx.ReadValue<float>();
        rotateValue = rotateValue switch
        {
            < -6 => -5,
            > 6 => 5,
            _ => rotateValue
        };
        
        //plusRotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));
        //targetRotation = origRotation * plusRotation;
        
    }

    private void InGameCamUpdate()
    {
        mainCamAnchor.DOMove(target.position, 0.8f).SetEase(Ease.Linear);
        var targetRotation = Quaternion.Euler(zoomXrot, rotateValue, 0);
        mainCamAnchor.DORotateQuaternion(targetRotation, 0.3f).SetEase(defaultEase);
    }

    public void MoveCamEase(Vector3 targetPos) => mainCamAnchor.DOMove(targetPos, 0.6f).SetEase(Ease.InSine);
    public void MoveCamInstant(Vector3 targetPos) =>  mainCamAnchor.DOMove(targetPos, 0f).SetEase(Ease.Linear);

    private void Update()
    {
        if (isCutscene || Vector3.Distance(mainCam.transform.position, target.position) <= 10)  //현재 컷신이거나 카메라와 플레이어 사이 거리가 10 이하일 경우
        {
            return;
        }
        InGameCamUpdate();
        
        /* 카메라와 카메라 anchor의 위치, 회전 보간 */
        // mainCam.DOOrthoSize(viewSize, 0.4f).SetEase(defaultEase);
        
    }
    
    public void SaveZoomRange(int editedRange)
    {
        savedZoomRange = zoomRange;
        // Debug.Log("저장된 카메라 줌 값: " + savedZoomRange);
        zoomRange = editedRange;
    }
    
    public void ReturnInteractionView() =>  zoomRange = savedZoomRange;

    // public void MakeMinigameView()
    // {
    //     main.DORect(new Rect(0, 0, 0.5f, 1), 0.8f).SetEase(Ease.OutQuart);
    //     mini.DORect(new Rect(0.5f, 0, 0.5f, 1), 0.8f).SetEase(Ease.OutQuart);
    // }

    // public void ReturnMinigameView()
    // {
    //     main.DORect(mainOrigRect, 1f).SetEase(Ease.OutQuart);
    //     mini.DORect(miniOrigRect, 1f).SetEase(Ease.OutQuart);
    // }



}
