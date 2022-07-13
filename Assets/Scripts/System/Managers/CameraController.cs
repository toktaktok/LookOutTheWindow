using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
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

    public Camera main; //메인 카메라 
    public Camera mini; //미니게임용 카메라

    [HideInInspector]
    public Rect mainOrigRect;
    [HideInInspector]
    public Rect miniOrigRect;


    void Start()
    {
        //cameraZ = 3f;

        target = GameObject.FindWithTag("Player").GetComponent<Transform>();

        zoomXrot = -23;
        mainOrigRect = main.rect;
        miniOrigRect = mini.rect;
        
    }

    public void Zoom(InputAction.CallbackContext ctx)
    {
        zoomRange -= ctx.ReadValue<float>() * 0.005f;

        if (zoomRange > 1)
            zoomRange = 1;
        else if (zoomRange < 0)
            zoomRange = 0;
    }

    public void Rotate(InputAction.CallbackContext ctx)
    {

        if (ctx.performed)
        {
            rotateValue += ctx.ReadValue<float>();

            if (rotateValue < -11)
                rotateValue = -10;
            else if (rotateValue > 11)
                rotateValue = 10;
        }


        //plusRotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));
        //targetRotation = origRotation * plusRotation;
        
    }

    private void InGameCamUpdate()
    {
        mainCamAnchor.DOMove(target.position, 0.8f).SetEase(Ease.Linear);
        var targetRotation = Quaternion.Euler(zoomXrot, rotateValue, 0);
        mainCamAnchor.DORotateQuaternion(targetRotation, 0.3f).SetEase(defaultEase);


    }

    public void MoveCamEase(Vector3 targetPos)
    {
        Debug.Log("cam move ease");
        mainCamAnchor.DOMove(targetPos, 0.6f).SetEase(Ease.InSine);
    }
    public void MoveCamInstant(Vector3 targetPos)
    {
        mainCamAnchor.DOMove(targetPos, 0f).SetEase(Ease.Linear);
    }

    private void Update()
    {
        if (!isCutscene)
        {
            InGameCamUpdate();
        }
        
        // fovSize = 30 * (1 + zoomRange);
        // zoomXrot = 335 + (20 * zoomRange);
        
        
        /* 카메라와 카메라 anchor의 위치, 회전 보간 */
        // main.DOFieldOfView(fovSize, 0.4f).SetEase(defaultEase);
        main.DOOrthoSize(viewSize, 0.4f).SetEase(defaultEase);

        // mainCamAnchor.rotation = Quaternion.Slerp(mainCamAnchor.rotation, targetRotation, 0.3f);
        // mainCamAnchor.position = Vector3.Lerp(mainCamAnchor.position, targetPos, 3 * Time.deltaTime);

    }
    
    public void SaveZoomRange(int editedRange)
    {
        savedZoomRange = zoomRange;
        // Debug.Log("저장된 카메라 줌 값: " + savedZoomRange);
        zoomRange = editedRange;
    }
    
    public void ReturnInteractionView()
    {
        // Debug.Log("카메라 뷰 원위치"); 
        zoomRange = savedZoomRange;
    }

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
