using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using UnityEngine;
using DG.Tweening;

public class IntroMovement : MonoBehaviour
{
    public GameObject snowman;
    public GameObject bus;
    [SerializeField] private CameraManager cameraManager;
    private Player player;

    private void Start()
    {
        player = snowman.GetComponent<Player>();
        StartCoroutine(IntroAnimation());
        cameraManager.isCutscene = true;
    }

    IEnumerator IntroAnimation()
    {
        
        // Move 1
        cameraManager.ModifyZoomRange(8);
        cameraManager.MoveCamInstant(new Vector3(-1, -1.7f, -8));  //고정 카메라 앵글
        Debug.Log("1");

        bus.transform.position = new Vector3(50, -1.5f, -9);    //버스 시작 위치
        snowman.transform.position = new Vector3(46, 4.8f, -8);  //눈사람 시작 위치
        cameraManager.MoveCamInstant(new Vector3(-1, -1.7f, -8));  //고정 카메라 앵글
        Debug.Log("2");


        bus.transform.DOMoveX(0f, 3f);                 // 버스 이동
        snowman.transform.DOMoveX(-4, 3f);             // 눈사람 이동
        cameraManager.MoveCamInstant(new Vector3(-1, -1.7f, -8));  //고정 카메라 앵글
        Debug.Log("3");


        yield return new WaitForSeconds(4);
        cameraManager.MoveCamInstant(new Vector3(-1, -1.7f, -8));  //고정 카메라 앵글
        Debug.Log("4");

        
        
        // Move 2
        player.SetIntroAnim();
        player.StartWalkAnim();
        player.MoveToDestLinear(new Vector3(-11, 2f, 0f), 10);
        
        yield return new WaitForSeconds(1);
        player.StopWalkAnim();  //눈사람 걸음 멈춤

        yield return new WaitForSeconds(1);
        bus.transform.DOMoveX(-100, 5f).SetEase(Ease.InSine);
        
        yield return null;
    }



}
