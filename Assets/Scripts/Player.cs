using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
//using System;
// using System.Collections.Generic;
// using System.Runtime.InteropServices.WindowsRuntime;
// using UnityEngine.UIElements;
// using System.Web.UI.WebControls;


/*
 * 클래스 이름: Player
 * 기능: 플레이어 캐릭터(Snowman)에게 붙는 스크립트.
 * 캐릭터의 조작, 애니메이션, 특정 범위 안에서의 상호작용 등을 관리한다.
 */
public class Player : MonoBehaviour
{
    private Vector2 moveValue = Vector2.zero;
    private int hMoveSpeed;
    private int vMoveSpeed;
    
    private SpriteRenderer _sprite;
    private Animator _anim;
    
    private Collider _interactingObject; //상호작용한 오브젝트
    
    private void Start()
    {
        hMoveSpeed = 30;  //횡 이동 스피드
        vMoveSpeed = 0;  //앞뒤 이동 스피드(현재 이동 X)
        _anim = gameObject.GetComponentInChildren<Animator>();   //animator 컴포넌트  
        _sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
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
    }

    /*
    * 함수 이름 : InputMove
    * 기능 : InputAction을 통해 방향키의 Vector를 받고 moveValue에 넘겨준다. 스프라이트 애니메이션 재생
    */
    public void InputMove(InputAction.CallbackContext ctx)
    {
        // if (_interactingObject) { return; }
        
        //애니메이션 parameter 변화.
        if (ctx.started)
        {
            _anim.SetBool("isMove", true);    
        }
        else if (ctx.canceled)
        {
            _anim.SetBool("isMove", false);  //애니메이션 정지
        }
        
        moveValue = ctx.ReadValue<Vector2>();
        _sprite.flipX = moveValue.x switch           //방향에 따라 스프라이트 반전
        {
            > 0 => false,
            < 0 => true,
            _ => _sprite.flipX
        };
    }
    
    /*
     * 함수 이름 : Interact
     * 기능 : InputAction을 통해 상호작용 할 시(input E) 미니게임이 있는지 확인한다.
     */
    public void Interact(InputAction.CallbackContext ctx)
    {
        //E를 눌렀을 시 interactingObject가 존재한다면
        if (!_interactingObject || !ctx.performed)
        {
            return;
        }
        // isInteracting = true;
        CameraController.Instance.SaveZoomRange(0);
        CheckInteractedObject(_interactingObject);
    }
    
    /*
     * 함수 이름 : StopWalkAnim
     * 기능 : 애니메이션의 isMove 프로퍼티 false
     */
    public void MoveToDestination(Vector3 dest, float moveSpeed)
    {
        var distance = Vector3.Distance(transform.position, dest);
        _anim.SetBool("isMove", true);
        transform.DOMove(dest, distance / moveSpeed).SetEase(Ease.Linear);
    }

    public void MoveYPosToMap()
    {
        // transform.DOMoveY();
    }
    
    /*
     * 함수 이름 : MoveHorizontal
     * 기능 : 인자로 받는 xValue만큼 X 이동
     */
    private void MoveHorizontal(float xValue)   //moveValue.x -> position.x
    {
        transform.DOMoveX(transform.position.x + xValue * hMoveSpeed * Time.fixedDeltaTime, 0.1f);
    }
    
    /*
     * 함수 이름 : MoveVertical
     * 기능 : 인자로 받는 yValue만큼 Z 이동
     */
    private void MoveVertical(float yValue)   //moveValue.y -> position.z
    {
        transform.DOMoveZ(transform.position.z + yValue * vMoveSpeed * Time.fixedDeltaTime, 0.1f);
    }

    /*
     * 함수 이름 : StartWalkAnim, StopWalkAnim
     * 기능 : 애니메이션의 isMove 프로퍼티 T/F set
     */
    public void StartWalkAnim() => _anim.SetBool("isMove", true);
    public void StopWalkAnim() => _anim.SetBool("isMove", false);

    /*
     * 함수 이름 : OnIntroAnim
     * 기능 : 애니메이션의 isIntro 프로퍼티 true
     */
    public void OnIntroAnim() => _anim.SetBool("isIntro", true);
    
    /*
     * 함수 이름 : IsMoving
     * 기능 : input action으로 읽은 moveValue가 0인지 체크하고, 아닐 시 true 반환
     * 반환값 : bool
     */
    private bool IsMoving() => moveValue != Vector2.zero;
    
    
    void CheckInteractedObject(Collider interacted) //오브젝트와 상호작용한다.(가능한 오브젝트에게)
    {
        interacted.SendMessage("Check");    //Villager/Item.cs 의 함수
    }
    
    private void OnTriggerEnter(Collider interacted)
    {
        UIManager.Instance.OpenInteractionKey(); //Trigger와 접촉할 시 상호작용 키를 보이게 한다.
        _interactingObject = interacted;         //접촉한 콜라이더가 존재할 시

        if (interacted.gameObject.layer == 10)
        {
            UIManager.Instance.OpenMapMovingUI();
        }

    }
    
    /*
     * 트리거에서 빠져나올 시 UIManager, CameraController가 가지고 있던 창이나 뷰 원위치
     */
    private void OnTriggerExit(Collider interacted)
    {
        if (!_interactingObject) { return; }
        
        _interactingObject = null;
        UIManager.Instance.CloseInteractionKey();    //상호작용 UI 닫기
        UIManager.Instance.CloseDialoguePopup();
        CameraController.Instance.ReturnInteractionView();  //카메라 줌 수치를 상호작용 전 시점으로 돌린다.
            
        //미니게임이 실행중이었다면?
        if (MinigameManager.Instance.IsMinigamePlaying())
        { 
            MinigameManager.Instance.CloseMinigameView();   //미니게임 창 닫기
        }

        if (interacted.gameObject.layer == 10)
        {
            UIManager.Instance.CloseMapMovingUI();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = new ContactPoint();
        contact = collision.GetContact(0);
        // Debug.Log(contact.point.normalized);
        
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
