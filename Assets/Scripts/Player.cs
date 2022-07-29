using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
//using Unity.VisualScripting;
//using System;
// using System.Collections.Generic;
// using System.Runtime.InteropServices.WindowsRuntime;
// using UnityEngine.UIElements;
// using System.Web.UI.WebControls;



//클래스 이름: Player
//기능: 플레이어 캐릭터(Snowman)에게 붙는 스크립트.
//캐릭터의 조작, 애니메이션, 특정 범위 안에서의 상호작용 등을 관리한다.
public class Player : MonoBehaviour
{
    
    private Vector2 moveValue = Vector2.zero;
    private int hMoveSpeed;                  //좌우 이동 속도
    private int vMoveSpeed;                  //상하 이동 속도
    
    private SpriteRenderer _sprite;
    private Animator _anim;
    [SerializeField] private Collider _interactingObject; //상호작용한 오브젝트
    [SerializeField] private bool canUsePassage = true;
    
    private void Start()
    {
        hMoveSpeed = 30;
        vMoveSpeed = 100;
        _anim = gameObject.GetComponentInChildren<Animator>();   //child의 animator 컴포넌트.
        _sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
    }


    private void LateUpdate()
    {
        switch (IsMoving()) //움직이고 있다면
        {
            case true:
                MoveHorizontal(moveValue.x);
                // MoveVertical(moveValue.y);
                break;
            case false:
                break;
        }
    }

    //이름 : InputMove
    //기능 : InputAction을 통해 방향키의 Vector를 받고 moveValue를 초기화. 맞춰 애니메이션 재생
    public void InputMove(InputAction.CallbackContext ctx)
    {
        moveValue = ctx.ReadValue<Vector2>();
        
        if (moveValue.y != 0) // 상하로 이동할 때
        {
            if (!_interactingObject)
            {
                return;
            }

            if ( _interactingObject.gameObject.layer == 10 && canUsePassage) // 트리거가 맵 이동과 연관되었다면?
            {
                MapManager.Instance.MoveToAnotherStreet(_interactingObject.gameObject.GetComponent<Passage>()); // 거리 이동
                StartCoroutine(CheckDelayTime());
            }
        }
        
        //애니메이션 parameter에 변화를 준다.
        if (ctx.started)
        {
            StartWalkAnim();
        }
        else if (ctx.canceled)
        {
            StopWalkAnim();
        }
        
        _sprite.flipX = moveValue.x switch //방향에 따라 스프라이트 반전
        {
            > 0 => false,
            < 0 => true,
            _ => _sprite.flipX
        };
    }
    
    private IEnumerator CheckDelayTime() //특정 행동을 무수히 반복하지 않도록 지연을 준다.
    {
        canUsePassage = false;
        yield return new WaitForSeconds(1f);
        canUsePassage = true;
        yield return null;
    }
    
 
     // 함수 이름 : Interact
     // 기능 : InputAction을 통해 상호작용 할 시(input E) 미니게임이 있는지 확인한다.
    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!_interactingObject || !ctx.performed) //E를 눌렀을 시 interactingObject가 존재한다면
        {
            return;
        }
        
        CameraController.Instance.SaveZoomRange(0);
        CheckInteractedObject(_interactingObject);
        moveValue = Vector2.zero;
    }
    
    public void MoveToDestination(Vector3 dest, float moveSpeed) //컷신에서 캐릭터를 움직일 때 사용
    {
        var distance = Vector3.Distance(transform.position, dest);
        _anim.SetBool("isMove", true);
        transform.DOMove(dest, distance / moveSpeed).SetEase(Ease.Linear); //속도 기반으로 목적지를 향해 움직이도록.
    }

    
    // 이름 : MoveHorizontal
    // 기능 : 인자로 받는 xValue만큼 X 이동
    private void MoveHorizontal(float xValue)   //moveValue.x -> position.x
    {
        transform.DOMoveX(transform.position.x + xValue * hMoveSpeed * Time.fixedDeltaTime, 0.1f);
    }
    
     // 이름 : MoveVertical
     // 기능 : 인자로 받는 yValue만큼 Z 이동
    private void MoveVertical(float yValue)   //moveValue.y -> position.z
    {
        transform.DOMoveZ(transform.position.z + yValue * vMoveSpeed * Time.fixedDeltaTime, 0.1f);
    }

    public void MoveStreet(float zPos)
    {
        transform.DOMoveZ(zPos, 0.5f);
        TimeWalkAnim(0.5f);
    }


    // 이름 : StartWalkAnim, StopWalkAnim
    // 기능 : 애니메이션의 isMove 프로퍼티 T/F set
    public void StartWalkAnim() => _anim.SetBool("isMove", true);
    public void StopWalkAnim() => _anim.SetBool("isMove", false);

    private IEnumerator TimeWalkAnim(float playTime)
    {
        StartWalkAnim();
        yield return new WaitForSeconds(playTime);
        StopWalkAnim();
    }


    // 이름 : OnIntroAnim
    // 기능 : 애니메이션의 isIntro 프로퍼티 true
    public void OnIntroAnim() => _anim.SetBool("isIntro", true);
    
    // 함수 이름 : IsMoving
    // 기능 : input action으로 읽은 moveValue가 0인지 체크하고, 아닐 시 true 반환
    // 반환값 : bool
    private bool IsMoving() => moveValue != Vector2.zero;
    
    
    void CheckInteractedObject(Collider interacted) //오브젝트와 상호작용한다.(가능한 오브젝트에게)
    {
        interacted.SendMessage("Check");    //Villager/Item.cs 의 함수
    }
    
    private void OnTriggerEnter(Collider interacted)
    {
        UIManager.Instance.OpenInteractionKey(); //Trigger와 접촉할 시 상호작용 키를 보이게 한다.
        _interactingObject = interacted;         //접촉한 콜라이더가 존재할 시

        if (interacted.gameObject.layer == 10)  //Map 관련 트리거 진입
        {
            UIManager.Instance.OpenMapMovingUI(interacted.gameObject.GetComponent<Passage>());
        }

    }
    
    // 트리거에서 빠져나올 시 UIManager, CameraController가 가지고 있던 창이나 뷰 원위치
    private void OnTriggerExit(Collider interacted)
    {
        if (!_interactingObject) { return; }
        
        _interactingObject = null;
        UIManager.Instance.CloseInteractionKey();    //상호작용 UI 닫기
        UIManager.Instance.CloseDialoguePopup();
        CameraController.Instance.ReturnInteractionView();  //카메라 줌 수치를 상호작용 전 시점으로 돌린다.
        
        if (interacted.gameObject.layer == 10)  //Map 관련 트리거에서 나옴
        {
            UIManager.Instance.CloseMapMovingUI();
        }
        //미니게임이 실행중이었다면?
        // if (MinigameManager.Instance.IsMinigamePlaying())
        // { 
        //     MinigameManager.Instance.CloseMinigameView();   //미니게임 창 닫기
        // }
        
    }

    private void OnCollisionEnter(Collision collision) //건물 벽 등 맵 가장자리에 충돌
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
