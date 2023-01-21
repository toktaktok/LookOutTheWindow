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

    private float _preViewSize;

    [HideInInspector]
    public Rect mainOrigRect;
    [HideInInspector]
    public Rect miniOrigRect;
    [SerializeField] private Ease defaultEase = Ease.OutSine;

    private Vector3 _prePosition;                      //다시 복귀할 위치 값 저장
    private Quaternion _preQuaternion;                  //다시 복귀할 회전 값 저장
    private float _savedZoomRange;                       //기존 줌 범위
    private float _rotateValue;                          //회전 값
    
    private bool isCutscene = false;                    //현재 컷신인지 확인하는 변수
    private bool _isFixed = false;                      //카메라 각도 고정
    
    private const float ZoomXRot = -24;                 //zoomRange에 따라 rotation.x를 조절하는 변수 / 기본 -23
    private readonly WaitForSeconds _camTimer = new WaitForSeconds(0.2f);


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

    //input action 대신 사용할 수 있는 방법으로.
    public void Rotate(InputAction.CallbackContext ctx)
    {
        if ( !ctx.performed ) { return; }   //callback 값 중 중간 값 하나만 받는다.
        
        _rotateValue += ctx.ReadValue<float>();
        _rotateValue = _rotateValue switch
        {
            < -3 => -2.5f,
            > 3 => 2.5f,
            _ => _rotateValue
        };
        var targetRotation = Quaternion.Euler(0, _rotateValue, 0);
        mainCamAnchor.DORotateQuaternion(targetRotation, 0.3f).SetEase(defaultEase);

    }
    
    private void Update()
    {
        //현재 카메라가 고정되어야 하거나(실내), 컷신이거나, 카메라와 플레이어 사이 거리가 10 이하일 경우 카메라 업데이트 X
        if (_isFixed || isCutscene || Vector3.Distance(mainCam.transform.position, target.position) <= 2)  
        {
            return;
        }
        
        InGameCamUpdate();
    }
     private void InGameCamUpdate()
    {
        // 카메라와 카메라 anchor의 위치, 회전 보간.
        //anchor y size = playerPos - 5 ~ playerPos + 13
        mainCamAnchor.DOMove(target.position + new Vector3(0, zoomRange * 6 - 4,0), 0.4f).SetEase(defaultEase);
        // mainCamAnchor.position = target.position;
        
        //업데이트에 줌 수치 넣었을 때
        if (viewSize - _preViewSize < 0.2f)
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
        _savedZoomRange = zoomRange;
        zoomRange = editedRange; //0.19 되도록
        viewSize = 2.2f; //2.2f 되도록
        // viewSize = zoomRange * 4 + 2;

        // mainCam.DOOrthoSize(viewSize, 0.4f).SetEase(defaultEase);
    }
    
    public void ReturnInteractionView()
    { 
        zoomRange = _savedZoomRange;
        viewSize = zoomRange * 4 + 2;

        // mainCam.DOOrthoSize(viewSize, 0.4f).SetEase(defaultEase);
    }  

    public void MakeMiniGameView()
    {
        mainCam.DORect(new Rect(0, 0, 0.5f, 1), 0.8f).SetEase(Ease.OutQuart);
        miniCam.DORect(new Rect(0.5f, 0, 0.5f, 1), 0.8f).SetEase(Ease.OutQuart);
    }

    public void InsideCamera(Vector3 pos, Quaternion rot)
    {
        _isFixed = true;
        var mainCamTf = mainCam.transform;
        _prePosition = mainCamTf.position;
        _preQuaternion = mainCamTf.rotation;
        MoveCamInstant(pos);
        mainCamTf.SetPositionAndRotation(pos, rot);
        // mainCam.transform.DORotateQuaternion(rot, 0f);
    }

    public void ReturnPreCameraView()
    {
        mainCam.transform.SetPositionAndRotation(_prePosition, _preQuaternion);
        _isFixed = false;
    }

    public void ReturnMiniGameView()
    {
        mainCam.DORect(mainOrigRect, 1f).SetEase(Ease.OutQuart);
        miniCam.DORect(miniOrigRect, 1f).SetEase(Ease.OutQuart);
    }

    private IEnumerator CamMoveTimer()
    {
        yield return _camTimer;
        _preViewSize = viewSize;
    }



}
