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
    public Transform mainCamAnchor; //카메라의 회전축
    public Camera mainCam; //메인 카메라 
    public Camera miniCam; //미니게임용 카메라
    
    [SerializeField]
    private float zoomRange = 0.5f;         //카메라 줌 범위를 조절하는 변수
    [Range(2, 4)]
    public float viewSize = 4;  //zoomRange에 따라 fieldOfView를 바꾸는 변수

    private float preViewSize;

    [HideInInspector]
    public Rect mainOrigRect;
    [HideInInspector]
    public Rect miniOrigRect;
    
    [SerializeField] private Ease defaultEase = Ease.OutSine;
    private float savedZoomRange;    //기존 줌 범위
    private float rotateValue;       //회전 값
    private bool isCutscene = false;
    private const float _zoomXRot = -24;                 //zoomRange에 따라 rotation.x를 조절하는 변수 / 기본 -23
    private readonly WaitForSeconds camTimer = new WaitForSeconds(0.2f);


    private void Start()
    {
        DOTween.SetTweensCapacity(2000, 50);
        if (GameObject.FindWithTag("Player").TryGetComponent<Transform>(out var tar))
        {
            target = tar;
        }
        mainOrigRect = mainCam.rect;
        miniOrigRect = miniCam.rect;
        zoomRange = 0.5f;
        viewSize = zoomRange * 4 + 2;


    }

    // public void Zoom(InputAction.CallbackContext ctx)
    // {
    //     zoomRange -= ctx.ReadValue<float>() * 0.005f; // 들어온 마우스 스크롤 값에 따라 변경
    //     zoomRange = zoomRange switch // zoomRange(0~1) 를 넘지 않도록 한다.
    //     {
    //         > 1 => 1,
    //         < 0 => 0,
    //         _ => zoomRange
    //     };
    //     viewSize = zoomRange * 16 + 8;
    // }

    public void Rotate(InputAction.CallbackContext ctx)
    {
        if ( !ctx.performed ) { return; }   //callback 값 중 중간 값 하나만 받는다.
        
        rotateValue += ctx.ReadValue<float>();
        rotateValue = rotateValue switch
        {
            < -3 => -2.5f,
            > 3 => 2.5f,
            _ => rotateValue
        };
        var targetRotation = Quaternion.Euler(0, rotateValue, 0);
        mainCamAnchor.DORotateQuaternion(targetRotation, 0.3f).SetEase(defaultEase);

    }
    
    private void FixedUpdate()
    {
        //현재 컷신이거나 카메라와 플레이어 사이 거리가 10 이하일 경우
        if (isCutscene || Vector3.Distance(mainCam.transform.position, target.position) <= 2)  
        {
            return;
        }
        InGameCamUpdate();
    }
     private void InGameCamUpdate()
    {
        // 카메라와 카메라 anchor의 위치, 회전 보간.
        //anchor y size = playerPos - 5 ~ playerPos + 13
        mainCamAnchor.DOMove(target.position + new Vector3(0, zoomRange * 6 - 3,0), 0.4f).SetEase(defaultEase);
        
        //업데이트에 줌 수치 넣었을 때
        if (viewSize - preViewSize < 0.2f)
        {
            return;
        }
        mainCam.DOOrthoSize(viewSize, 0.4f).SetEase(defaultEase);
    }
     
     //카메라를 움직여야 할 위치가 따로 있을 시
     public void MoveCamEase(Vector3 targetPos) => mainCamAnchor.DOMove(targetPos, 0.4f).SetEase(defaultEase);
     public void MoveCamInstant(Vector3 targetPos) =>  mainCamAnchor.DOMove(targetPos, 0f).SetEase(Ease.Linear);
     
    public void SaveZoomRange(float editedRange)
    {
        savedZoomRange = zoomRange;
        zoomRange = editedRange;
        viewSize = zoomRange * 4 + 2;

        // mainCam.DOOrthoSize(viewSize, 0.4f).SetEase(defaultEase);
    }
    
    public void ReturnInteractionView()
    { 
        zoomRange = savedZoomRange;
        viewSize = zoomRange * 4 + 2;

        // mainCam.DOOrthoSize(viewSize, 0.4f).SetEase(defaultEase);
    }  

    public void MakeMiniGameView()
    {
        mainCam.DORect(new Rect(0, 0, 0.5f, 1), 0.8f).SetEase(Ease.OutQuart);
        miniCam.DORect(new Rect(0.5f, 0, 0.5f, 1), 0.8f).SetEase(Ease.OutQuart);
    }

    public void ReturnMiniGameView()
    {
        mainCam.DORect(mainOrigRect, 1f).SetEase(Ease.OutQuart);
        miniCam.DORect(miniOrigRect, 1f).SetEase(Ease.OutQuart);
    }

    private IEnumerator CamMoveTimer()
    {
        yield return camTimer;
        preViewSize = viewSize;
    }



}
