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
    private int horizonMoveSpeed;
    private int verticalMoveSpeed;
    
    SpriteRenderer sprite;
    Animator anim;
    
    Collider interactingObject; //상호작용한 오브젝트
    
    private void Start()
    {
        horizonMoveSpeed = 50;
        verticalMoveSpeed = 0;
        anim = gameObject.GetComponentInChildren<Animator>();  
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    
    private void LateUpdate()
    {
        switch (IsMoving()) //움직이고 있다면
        {
            case true:
                MoveHorizontal(moveValue.x);
                MoveVertical(moveValue.y);
                break;
            case false:
                break;
        }
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

    /*
    * 함수 이름 : InputMove
    * 기능 : InputAction을 통해 방향키의 Vector를 받고 moveValue에 넘겨준다. 스프라이트 애니메이션 재생
    */
    public void InputMove(InputAction.CallbackContext ctx)
    {
        if (interactingObject) { return; }  //
        
        //애니메이션 parameter 변화.
        if (ctx.started)
        {
            anim.SetBool("isMove", true);    
        }
        else if (ctx.canceled)
        {
            anim.SetBool("isMove", false);  //애니메이션 정지
        }
        
        moveValue = ctx.ReadValue<Vector2>();
        sprite.flipX = moveValue.x switch           //방향에 따라 스프라이트 반전
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
    
    /*
     * 함수 이름 : MoveHorizontal
     * 기능 : 인자로 받는 xValue만큼 X 이동
     */
    public void MoveHorizontal(float xValue)   //moveValue.x -> position.x
    {
        transform.DOMoveX(transform.position.x + xValue * horizonMoveSpeed * Time.deltaTime, 0.1f);
    }
    
    /*
     * 함수 이름 : MoveVertical
     * 기능 : 인자로 받는 yValue만큼 Z 이동
     */
    public void MoveVertical(float yValue)   //moveValue.y -> position.z
    {
        transform.DOMoveZ(transform.position.z + yValue * verticalMoveSpeed * Time.deltaTime, 0.1f);
    }

    /*
     * 함수 이름 : StartWalkAnim
     * 기능 : 애니메이션의 isMove 프로퍼티 true
     */
    public void StartWalkAnim()
    {
        anim.SetBool("isMove", true);
    }

    /*
     * 함수 이름 : StopWalkAnim
     * 기능 : 애니메이션의 isMove 프로퍼티 false
     */
    public void StopWalkAnim()
    {
        anim.SetBool("isMove", false);
    }
    
    /*
     * 함수 이름 : IsMoving
     * 기능 : input action으로 읽은 moveValue가 0인지 체크하고, 아닐 시 true 반환
     * 반환값 : bool
     */
    private bool IsMoving()
    {
        return moveValue != Vector2.zero;
    }
    
    /*
     * 함수 이름 : OnIntroAnim
     * 기능 : 애니메이션의 isIntro 프로퍼티 true
     */
    public void OnIntroAnim()
    {
        anim.SetBool("isIntro", true);
    }

    /*
     * 함수 이름 : StopWalkAnim
     * 기능 : 애니메이션의 isMove 프로퍼티 false
     */
    public void MoveToDestination(Vector3 dest, float moveSpeed)
    {
        var distance = Vector3.Distance(transform.position, dest);
        anim.SetBool("isMove", true);
        transform.DOMove(dest, distance / moveSpeed).SetEase(Ease.Linear);
        
    }

    public IEnumerator TestCoroutine()
    {
        yield return null;
    }
    
    void CheckInteractedObject(Collider interacted)
    {
        interacted.SendMessage("Check");
    }
    
    private void OnTriggerEnter(Collider interacted)
    {
        UIManager.Instance.OpenInteractionButton(); //Trigger와 접촉할 시 상호작용 키를 보이게 한다.
        interactingObject = interacted;         //접촉한 콜라이더가 존재할 시
    }
    
    /*
     * 트리거에서 빠져나올 시 UIManager, CameraController가 가지고 있던 창이나 뷰 원위치
     */
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
            // isColliding = true;
            Debug.Log("Colliding!");
        }
    }
    

    private void OnCollisionExit(Collision collision)
    {
        // isColliding = false;
    }









}
