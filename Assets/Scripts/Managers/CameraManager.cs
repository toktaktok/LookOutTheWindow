using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class CameraManager : Singleton<CameraManager>
{
    public Transform target;        //카메라 타겟(플레이어 기본)
    public Position targetPos;
    public Camera mainCam;      //메인 카메라 
    public Camera miniCam;      //미니게임용 카메라
    [SerializeField] private Transform mainCamAnchor;       //카메라의 회전축. inspector에서 초기화했으므로 SerializeField 유지
    

    [Range(0, 6)]
    public int zoomRange = 4;
    private int _preZoomRange;
    private int _savedZoomRange;
    // private float _viewSize = 4;        //zoomRange에 따라 fieldOfView에 적절한 수치를 보정한다.
    // private float _preViewSize;
    private const int BaseZoomRange = 4;

    
    [HideInInspector] public Rect mainOrigRect;
    [HideInInspector] public Rect miniOrigRect;

    [SerializeField] private Quaternion baseQuaternion;     //베이스 회전 값
    private Vector3 _prePosition;                      //복귀할 position
    private Quaternion _preQuaternion;                  //복귀할 quaternion
    private float _rotateValue;
    
    //TODO: 맵 범위에 맞춰 배경 위치가 확실히 다르게 보이도록 보정 필요. 현재 맵에서의 위치 범위를 만들어 놓고 배경 스프라이트는 그에 맞춰 조금씩 이동되도록 만들어야 함
    private GameObject[] BGSprites;
    private readonly float[] _direction = { 0.2f, 0.4f, 0.6f };
    
    public bool isCutscene = false;     //현재 컷신인지 확인
    [SerializeField] private bool _isFixed;      //카메라 고정

    private readonly float baseDuration = 0.5f;
    private const float ZoomXRot = -24;     //zoomRange에 따라 rotation.x를 조절하는 변수 / 기본 -23
    private readonly WaitForSeconds _camTimer = new WaitForSeconds(0.2f);
    private const Ease DefaultEase = Ease.OutSine;

    private void Start()
    {
        GameManager.Input.keyAction += OnKeyboard;
        SetInitialProperties();
        
        InGameCamPosUpdate();
    }

    private void Update()
    {
        //현재 카메라가 고정되어야 하거나(실내), 컷신이거나, 카메라와 플레이어 사이 거리가 10 이하일 경우 카메라 업데이트 X
        if (_isFixed || isCutscene || Vector3.Distance(mainCamAnchor.position, target.position) <= 0.2f)  
        {
            return;
        }
        
        InGameCamPosUpdate();
    }
    
     private void InGameCamPosUpdate() //카메라 위치 업데이트.
    {
        // 카메라와 카메라 anchor의 위치 보간.
        //anchor y size = playerPos - 5 ~ playerPos + 13
        // mainCamAnchor.DOMove(target.position + new Vector3(0, -1.1f,0), 0.4f).SetEase(DefaultEase);
        MoveCamEase(target.position);
        
        if (_preZoomRange != zoomRange)
        {
            UpdateOrthoSize();
        }
        _preZoomRange = zoomRange;
    }
     
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
         var targetRotation = baseQuaternion * Quaternion.Euler(0, _rotateValue, 0);
         mainCamAnchor.DORotateQuaternion(targetRotation, 0.5f).SetEase(DefaultEase);
     }
     
     //카메라 움직여야 할 위치가 정해져 있을 시 사용
     public void MoveCamEase(Vector3 pos) => mainCamAnchor.DOMove(pos + new Vector3(0, zoomRange * 0.7f -4 ,0), baseDuration).SetEase(DefaultEase);
     public void MoveCamInstant(Vector3 pos) =>  mainCamAnchor.DOMove(pos + new Vector3(0, zoomRange * 0.7f -4 ,0), 0f);


     
     public void ReturnInteractionView() //상호작용 종료 시 기존 줌 퍼센트로 돌아감
     { 
         zoomRange = _savedZoomRange;
         UpdateOrthoSize();
     }

     public void ModifyZoomRange(int editedRange)
    {
        _savedZoomRange = zoomRange;
        zoomRange = editedRange;
        UpdateOrthoSize();
    }



    public IEnumerator InsideCamera(Transform tf)
    {
        var mainCamTransform = mainCam.transform;
        _prePosition = mainCamTransform.position;
        _preQuaternion = baseQuaternion;
        baseQuaternion = tf.rotation;
        _isFixed = true;
        MoveCamInstant(tf.position);
        mainCamAnchor.transform.rotation = baseQuaternion;
        _isFixed = false;
        yield return null;
    }
    
     public void MakeMiniGameView()
    {
        mainCam.DORect(new Rect(0, 0, 0.5f, 1), 0.8f).SetEase(Ease.OutQuart);
        miniCam.DORect(new Rect(0.5f, 0, 0.5f, 1), 0.8f).SetEase(Ease.OutQuart);
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
        ReturnInteractionView();
    }

    private void SetInitialProperties()
    {
        DOTween.SetTweensCapacity(2000, 50);
        SetTarget("Player");
        mainOrigRect = mainCam.rect;
        miniOrigRect = miniCam.rect;
        baseQuaternion = Quaternion.Euler(0, 0, 0);

        zoomRange = BaseZoomRange; //카메라 줌 정도(0 ~ 10)
        UpdateOrthoSize();
        
        BGSprites = UIManager.Instance.backGroundSprites;
    }
    
    private void SetTarget(string targetName)
    {
        if (GameObject.FindWithTag(targetName).TryGetComponent<Transform>(out var tar))
        {
            target = tar;
        }
    }
    
    private void UpdateOrthoSize()
    { 
        mainCam.DOOrthoSize(zoomRange *0.8f +1, baseDuration).SetEase(DefaultEase);
        MoveCamEase(target.position);
    }

    private void OnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isFixed = _isFixed switch
            {
                true => false,
                false => true
            };
        }
    }
}
