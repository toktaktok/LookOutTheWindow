using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CameraController : Singleton<CameraController>
{

    public Transform target; //타켓으로 할 대상 

    [Range(0, 1)]
    public float zoomRange;
    public float rotateValue;

    [Range(30, 60)]
    public float zoomSize;

    [Range(-25, 0)]
    float zoomXrot;

    public Transform mainCamAnchor;

    public Camera main; //메인 카메라 
    public Camera mini; //미니게임용 카메라

    Rect mainOrigRect;
    Rect miniOrigRect;


    void Start()
    {
        //cameraZ = 3f;

        zoomSize = 30f;
        zoomXrot = -25;
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

        rotateValue += ctx.ReadValue<float>();


        if (rotateValue < -11)
            rotateValue = -10;
        else if (rotateValue > 11)
            rotateValue = 10;

        //plusRotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));
        //targetRotation = origRotation * plusRotation;
        
    }

    void FixedUpdate()
    {

        zoomSize = 30 * (1 + zoomRange);

        zoomXrot = 335 + (20 * zoomRange);

        Vector3 targetPos = target.position;
        Quaternion targetRotation = Quaternion.Euler(zoomXrot, rotateValue, 0);

        mainCamAnchor.rotation = Quaternion.Slerp(mainCamAnchor.rotation, targetRotation, 0.3f);
        main.fieldOfView = Mathf.Lerp(main.fieldOfView, zoomSize, 4 * Time.deltaTime);
        mainCamAnchor.position = Vector3.Lerp(mainCamAnchor.position, targetPos, 3 * Time.deltaTime);

        // Vector3 targetPos = new Vector3(target.position.x, target.position.y + 12, target.position.z - 25);
        //mainCamAnchor.transform.DOMove(targetPos, 1f).SetEase(Ease.InQuart);
        //mainCamAnchor.DORotateQuaternion(targetRotation, 1f).SetEase(Ease.InSine);

    }

    public void MakeMinigameView()
    {
        main.DORect(new Rect(0, 0, 0.5f, 1), 0.8f).SetEase(Ease.OutQuart);
        //mini.gameObject.SetActive(true);
        mini.DORect(new Rect(0.5f, 0, 0.5f, 1), 0.8f).SetEase(Ease.OutQuart);
    }

    public void ReturnMinigameView()
    {
        main.DORect(mainOrigRect, 0.8f).SetEase(Ease.OutQuart);
        
        mini.DORect(miniOrigRect, 0.8f).SetEase(Ease.OutQuart);
        //mini.targetTexture = null;

        //mini.DORect(miniOrigRect, 0.8f).SetEase(Ease.OutQuart).OnComplete(() => {
        //    mini.gameObject.SetActive(false);
        //}); ;
    }
}
