using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player : MonoBehaviour
{

    Vector2 moveValue;
    int moveSpeed = 10;
    SpriteRenderer sprite;

    Animator anim;

    //bool isStaying = false;

    Collider interactingObject; //상호작용한 오브젝트

    private void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();  
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>(); 
    }

    private void Update()
    {
        if (moveValue == Vector2.zero) //이동하고 있지 않다면
        {
            anim.StopPlayback();
            anim.SetBool("isMove", false);  //애니메이션 정지
        }
            
    }

    
    public void FixedUpdate()
    {
        //플레이어 이동 수치 계산 
        transform.Translate
            (new Vector3(moveValue.x, 0, moveValue.y) * moveSpeed * Time.deltaTime);
               
    }


    /*
     * 함수 이름 : Move
     * 기능 : InputAction을 통해 방향키의 Vector를 받고 moveValue에 넘겨준다. 스프라이트 애니메이션 재생
     */
    public void Move(InputAction.CallbackContext ctx)
    {
        moveValue = ctx.ReadValue<Vector2>();
        anim.SetBool("isMove", true);
        //Debug.Log(moveValue);

        //방향에 따라 스프라이트 반전
        if (moveValue.x > 0)
        {
            sprite.flipX = false;
            
        }
        else if (moveValue.x < 0)
        {
            sprite.flipX = true;
        }

    }

    /*
     * 함수 이름 : Interact
     * 기능 : InputAction을 통해 상호작용 할 시(input E) 미니게임이 있는지 확인한다.
     */
    public void Interact(InputAction.CallbackContext ctx)
    {
        
        if(interactingObject != null && ctx.performed)   //E를 눌렀을 시 interactingObject가 존재한다면
        {
            CameraController.Instance.SaveZoomRange(0);
        }
    }



    void OnTriggerEnter(Collider interacted)
    {
        UIManager.Instance.OpenInteractionButton(); //Trigger와 접촉할 시 상호작용 키를 보이게 한다.
        interactingObject = interacted;         //접촉한 콜라이더가 존재할 시

    }


    void OnTriggerExit(Collider interacted)
    {
        
        interactingObject = null;

        
        UIManager.Instance.CloseInteractionUI();
        CameraController.Instance.ReturnInteractionView();  //카메라 원래 화면으로 돌리기

        if (MinigameManager.Instance.IsMinigamePlaying())
        {
            MinigameManager.Instance.CloseMinigameView();
            CameraController.Instance.ReturnMinigameView();     //미니게임 카메라 닫기
            
        }

    }


    void CheckInteractedObject(Collider interacted)
    {
        interacted.SendMessage("Check");
    }







}
