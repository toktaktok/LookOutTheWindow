using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using UnityEngine;
using DG.Tweening;

public class IntroMovement : MonoBehaviour
{
    public GameObject snowman;
    public GameObject bus;
    private Player player;
    
    private void Start()
    {
        player = snowman.GetComponent<Player>();
        StartCoroutine(IntroAnimation());
    }

    IEnumerator IntroAnimation()
    {
        WaitForSeconds waitFor1Seconds = new WaitForSeconds(1f);
        Debug.Log("인트로 컷신 시작");
        
        bus.transform.position = new Vector3(50, -1.5f, -9);    //버스 시작 위치
        snowman.transform.position = new Vector3(46, 4.8f, -8);  //눈사람 시작 위치

        CameraController.Instance.MoveCamInstant(new Vector3(-1.19f, 13, -5));  //고정 카메라 앵글
        
        bus.transform.DOMoveX(0f, 3f);                 // 버스 이동
        snowman.transform.DOMoveX(-4f, 3f);             // 눈사람 이동

        yield return waitFor1Seconds;
        
        player.OnIntroAnim();
        player.StartCoroutine(MoveToDestination( new Vector3(-11, 2.4f, 1.6f), 10 ) );
        
        yield return null;
    }



}
