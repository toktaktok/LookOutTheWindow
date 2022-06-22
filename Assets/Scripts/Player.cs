using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

/*
 * 클래스 이름: Player
 * 기능: 플레이어 캐릭터(snowman)에게 붙는 스크립트.
 * 캐릭터의 조작, 애니메이션, 특정 범위 안에서의 상호작용 등을 관리한다.
 */
public class Player : MonoBehaviour
{
    Vector2 moveValue;
    int moveSpeed = 10;
    SpriteRenderer sprite;

    Animator anim;
    public bool isInteracting = false;
    private Rigidbody _rigid;

    //bool isStaying = false;

    Collider interactingObject; //상호작용한 오브젝트

    private void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();  
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        _rigid = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (moveValue == Vector2.zero) //이동하고 있지 않다면
        {
            anim.SetBool("isMove", false);  //애니메이션 정지
        }
        //플레이어 이동 수치 계산 
        // _rigid.MovePosition
        //     ( moveSpeed * Time.deltaTime * transform.position + new Vector3(moveValue.x, 0, moveValue.y ));
        
        transform.Translate
            ( moveSpeed * Time.deltaTime * new Vector3(moveValue.x, 0, moveValue.y));
    }



    /*
     * 함수 이름 : Move
     * 기능 : InputAction을 통해 방향키의 Vector를 받고 moveValue에 넘겨준다. 스프라이트 애니메이션 재생
     */
    public void Move(InputAction.CallbackContext ctx)
    {
        if (!isInteracting)
        {
            moveValue = ctx.ReadValue<Vector2>();
        
            anim.SetBool("isMove", true);   //애니메이션 parameter 변화. 애니메이션 더 효과적으로 바꾸는 방법 리서치 필요

            //방향에 따라 스프라이트 반전
            // if (moveValue.x > 0){
            //     sprite.flipX = false;
            // }
            // else if (moveValue.x < 0){
            //     sprite.flipX = true;
            // }

            switch (moveValue.x) {
                case > 0:
                    sprite.flipX = false;
                    break;
                case < 0:
                    sprite.flipX = true;
                    break;
            }
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
            isInteracting = true;
            CameraController.Instance.SaveZoomRange(0);
            CheckInteractedObject(interactingObject);
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
        UIManager.Instance.CloseInteractionButton();    //상호작용 UI 닫기
        
        
        if (isInteracting)
        {
            UIManager.Instance.CloseDialoguePopup();
            CameraController.Instance.ReturnInteractionView();  //카메라 줌 수치를 상호작용 전 시점으로 돌린다.
            isInteracting = false;
            
            //미니게임이 실행중이었다면?
            if (MinigameManager.Instance.IsMinigamePlaying()) { 
                MinigameManager.Instance.CloseMinigameView();   //미니게임 창 닫기
            }
        }
    }

    void CheckInteractedObject(Collider interacted)
    {
        interacted.SendMessage("Check");
    }







}
