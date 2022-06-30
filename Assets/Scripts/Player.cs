using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.UI.WebControls;
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
    // Vector2 moveVector;
    private Vector2 moveValue = Vector2.zero;
    private Vector3 prevVector;
    int moveSpeed = 10;
    SpriteRenderer sprite;

    Animator anim;
    public bool isInteracting = false;
    private Rigidbody rigid;

    //bool isStaying = false;
    private bool isColliding = false;

    Collider interactingObject; //상호작용한 오브젝트
    private Transform mySprite;
    

    private void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();  
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        rigid = gameObject.GetComponent<Rigidbody>();
        mySprite = transform.GetChild(0);
    }

    private void Update()
    {

        //플레이어 이동 수치 계산 
        // _rigid.MovePosition
        //     ( moveSpeed * Time.deltaTime * transform.position + new Vector3(moveValue.x, 0, moveValue.y ));
        
    }

    private bool CheckMove()
    {
        return moveValue == Vector2.zero;
    }

    private void LateUpdate()
    {

        
        if (moveValue == Vector2.zero) //이동하고 있지 않다면
        {
            anim.SetBool("isMove", false);  //애니메이션 정지
        }
        // transform.Translate
        //     ( moveSpeed * Time.deltaTime * new Vector3(moveVector.x, 0, moveVector.y));


        // mySprite.Translate(-moveSpeed * Time.deltaTime * new Vector3(moveVector.x, 0, moveVector.y));

        // if (isColliding)
        // {
        //     transform.position = prevVector;
        // }
        // else
        // {
        //     mySprite.Translate(moveSpeed * Time.deltaTime * new Vector3(moveVector.x, 0, moveVector.y));
        // }
        //
        // prevVector = transform.position;

        // mySprite.localPosition = isColliding switch
        // {
        //     true => -moveSpeed * Time.deltaTime * new Vector3(moveVector.x, 0, moveVector.y),
        //     false => mySprite.localPosition
        // };

    }

    private void MoveHorizontal(float moveValue)
    {
        Debug.Log("횡 이동");
        transform.Translate( moveValue * moveSpeed * Time.deltaTime * Vector3.right );
    }

    private IEnumerator MoveH(float moveValue)
    {
        Debug.Log("horizontal move");
        while (CheckMove())
        {
            transform.Translate( moveValue * moveSpeed * Time.deltaTime * Vector3.right );
        }
        yield return null;
    }
    
    private IEnumerator MoveV(float moveValue)
    {
        Debug.Log("horizontal move");
        while (CheckMove())
        {
            transform.Translate( moveValue * moveSpeed * Time.deltaTime * Vector3.right );
        }
        yield return null;
    }

    private void MoveVertical(float moveValue)
    {
        Debug.Log("앞뒤 이동");
        transform.Translate(  moveValue * moveSpeed * Time.deltaTime * Vector3.forward );
    }

    private void MoveToDestination(Vector3 dest)
    {
        
    }

    public IEnumerator CutsceneMove(Vector3 target)
    {
        while(true)
        {
            transform.Translate
                ( moveSpeed * Time.deltaTime * target);
            
            if (Vector3.Distance(transform.position, target) < 0.2f)
            {
                yield return null;
            }
        }

    }

    public void PlayerAnim()
    {
        
    }


    /*
     * 함수 이름 : InputMove
     * 기능 : InputAction을 통해 방향키의 Vector를 받고 moveValue에 넘겨준다. 스프라이트 애니메이션 재생
     */
    public void InputMove(InputAction.CallbackContext ctx)
    {
        if (interactingObject) { return; }

        Debug.Log("move input 입력");
        
        // moveVector = ctx.ReadValue<Vector2>();
        
        moveValue = ctx.ReadValue<Vector2>();
        MoveHorizontal(moveValue.x);
        StartCoroutine(MoveH(moveValue.x));
        StartCoroutine(MoveV(moveValue.y));
        MoveVertical(moveValue.y);
        
        anim.SetBool("isMove", true);   //애니메이션 parameter 변화. 애니메이션 더 효과적으로 바꾸는 방법 리서치 필요
        //방향에 따라 스프라이트 반전
        sprite.flipX = moveValue.x switch
        {
            > 0 => false,
            < 0 => true,
            _ => sprite.flipX
        };


    }


    /*
     * 함수 이름 : Interact
     * 기능 : InputAction을 통해 상호작용 할 시(input E) 미니게임이 있는지 확인한다.
     */
    public void Interact(InputAction.CallbackContext ctx)
    {
        //E를 눌렀을 시 interactingObject가 존재한다면
        if (!interactingObject || !ctx.performed)
        {
            return;
        }
        
        // isInteracting = true;
        CameraController.Instance.SaveZoomRange(0);
        CheckInteractedObject(interactingObject);
        
    }



    private void OnTriggerEnter(Collider interacted)
    {
        UIManager.Instance.OpenInteractionButton(); //Trigger와 접촉할 시 상호작용 키를 보이게 한다.
        interactingObject = interacted;         //접촉한 콜라이더가 존재할 시
    }
    
    
    private void OnTriggerExit(Collider interacted)
    {
        if (!interactingObject)
        {
            return;
        }
        
        interactingObject = null;
        UIManager.Instance.CloseInteractionButton();    //상호작용 UI 닫기
        UIManager.Instance.CloseDialoguePopup();
        CameraController.Instance.ReturnInteractionView();  //카메라 줌 수치를 상호작용 전 시점으로 돌린다.
        isInteracting = false;
            
        //미니게임이 실행중이었다면?
        if (MinigameManager.Instance.IsMinigamePlaying())
        { 
            MinigameManager.Instance.CloseMinigameView();   //미니게임 창 닫기
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = new ContactPoint();
        contact = collision.GetContact(0);
        Debug.Log(contact.point.normalized);
        
        if (collision.gameObject.layer == 12)
        {
            isColliding = true;
            Debug.Log("Colliding!");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }

    void CheckInteractedObject(Collider interacted)
    {
        interacted.SendMessage("Check");
    }







}
