using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using EnumTypes;
using VoxelImporter;



//클래스 이름: Player
//기능: 플레이어 캐릭터(Snowman)에게 붙는 스크립트.
//캐릭터의 조작, 애니메이션, 특정 범위 안에서의 상호작용 등을 관리한다.
public class Player : MonoBehaviour
{

    private Vector2 moveValue = Vector2.zero;
    [SerializeField] private int hMoveSpeed; //좌우 이동 속도
    private int origSpeed;                   //처음 속도
    private int vMoveSpeed;                  //상하 이동 속도
    private Vector3 prevPos;
    private SpriteRenderer _sprite;
    private Animator _anim;
    [SerializeField] private Collider interactingObject; //상호작용한 오브젝트
    [SerializeField] private bool isCollided = false;
    [SerializeField] private bool canUsePassage = true;
    
    private void Start()
    {
        hMoveSpeed = 15; // 가로 이동 속도 (translate)
        // hMoveSpeed = 30; // 가로 이동 속도 (doMove)
        vMoveSpeed = 100;
        origSpeed = hMoveSpeed;
        moveValue = Vector2.zero;
        _anim = gameObject.GetComponentInChildren<Animator>();   //child의 animator 컴포넌트.
        _sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        prevPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (!IsMoving()) //이동 입력이 없으면 업데이트 하지 않음.
        {
            return;
        }
    
        if (!isCollided) // 충돌하지 않을 때 이전 position 저장.
        {
            prevPos = transform.position;
        }
        MoveHorizontal(moveValue.x); // 먼저 이동
    }

    private void LateUpdate()
    {
        if (isCollided) // 충돌했는가?
        {
            transform.position = prevPos; // 이전 위치로 이동
            isCollided = false; // 충돌 처리 false
        }
        else
        {
            // hMoveSpeed = origSpeed;
        }
    }

    //이름 : InputMove
    //기능 : InputAction을 통해 방향키의 Vector를 받고 moveValue를 초기화. 맞춰 애니메이션 재생
    public void InputMove(InputAction.CallbackContext ctx)
    {
        moveValue = ctx.ReadValue<Vector2>();
        
        if (moveValue.y != 0) // 상하로 이동할 때
        {
            if (!interactingObject)
            {
                return;
            }

            if ( interactingObject.gameObject.layer == 10 && canUsePassage ) // 트리거가 맵 이동과 연관되었다면?
            {
                MapManager.Instance.MoveToAnotherStreet(interactingObject.gameObject.GetComponent<Passage>()); // 거리 이동
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
    
    //특정 행동을 무수히 반복하지 않도록 지연을 준다.
    private IEnumerator CheckDelayTime()
    {
        canUsePassage = false;
        yield return new WaitForSeconds(0.5f);
        canUsePassage = true;
        yield return null;
    }


    // 함수 이름 : Interact
    // 기능 : InputAction을 통해 상호작용 할 시(input E) 미니게임이 있는지 확인한다.
    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!interactingObject || !ctx.performed) //E를 눌렀을 시 interactingObject가 존재한다면
        {
            return;
        }
        
        CameraController.Instance.SaveZoomRange(0);
        CheckInteractedObject(interactingObject);
        moveValue = Vector2.zero;
    }
    
    public void Jump()
    {
        StartCoroutine(StopSpeedForTime(0.3f));
        _anim.Play("Jump");
    }
    
    //목표 지점으로 캐릭터를 움직인다. (컷신 용)
    public void MoveToDestination(Vector3 dest, float moveSpeed)
    {
        var distance = Vector3.Distance(transform.position, dest);
        _anim.SetBool("isMove", true);
        transform.DOMove(dest, distance / moveSpeed).SetEase(Ease.Linear); //속도 기반으로 목적지를 향해 움직이도록.
    }

    // 이름 : MoveHorizontal
    // 기능 : 인자로 받는 xValue만큼 X 이동
    private void MoveHorizontal(float xValue)   //moveValue.x -> position.x
    {
        // transform.Translate( new Vector3
        //     (xValue * hMoveSpeed * Time.fixedDeltaTime, 0, 0));
        transform.Translate(new Vector3
            (xValue * hMoveSpeed * Time.fixedDeltaTime, 0, 0));
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
    
    // 이름 : IsMoving
    // 기능 : moveValue가 0인지 체크하고, bool 반환
    private bool IsMoving() => moveValue != Vector2.zero;
    
    
    void CheckInteractedObject(Collider interacted) //오브젝트와 상호작용한다.(가능한 오브젝트에게)
    {
        interacted.SendMessage("Check");    //Villager/Item.cs 의 함수
    }

    //interactingObject를 지운다.
    public void EraseInteractingObject() => interactingObject = null;

    public void SwitchSpeed(bool isStop)
    {
        hMoveSpeed = isStop switch
        {
            true => 0,
            false => origSpeed
        };
    }

    private IEnumerator StopSpeedForTime(float time)
    {
        hMoveSpeed = 0;
        yield return new WaitForSeconds(time);
        hMoveSpeed = origSpeed + 6;
        yield return new WaitForSeconds(time);
        hMoveSpeed = origSpeed;
        yield return null;
    }


    private void OnTriggerEnter(Collider interacted)
    {
        interactingObject = interacted;         //접촉한 콜라이더가 존재할 시
        switch (interacted.gameObject.layer)
        {
            case GlobalVariables.LayerNumber.character:
                UIManager.Instance.OpenInteractionKey(); //Trigger와 접촉할 시 상호작용 키를 보이게 한다.
                break;
            case GlobalVariables.LayerNumber.map: //Map 관련 트리거 진입
                UIManager.Instance.OpenMapMovingUI(interacted.gameObject.GetComponent<Passage>());
                break;
        }
    }

    private void OnTriggerStay(Collider interacted)
    {
        if (interactingObject)
        {
            return;
        }
        interactingObject = interacted;
    }

    // 트리거에서 빠져나올 시 UIManager, CameraController가 가지고 있던 창이나 뷰 원위치
    private void OnTriggerExit(Collider interacted)
    {
        interactingObject = null;
        switch (interacted.gameObject.layer)
        {
            case GlobalVariables.LayerNumber.character:
                UIManager.Instance.CloseInteractionKey();    //상호작용 UI 닫기
                break;
            case GlobalVariables.LayerNumber.map:            //Map 관련 트리거 진입
                UIManager.Instance.CloseMapMovingUI();
                break;
        }
        // UIManager.Instance.CloseDialoguePopup();
        // CameraController.Instance.ReturnInteractionView();  //카메라 줌 수치를 상호작용 전 시점으로 돌린다.
        
        //미니게임이 실행중이었다면?
        // if (MiniGameManager.Instance.IsMiniGamePlaying())
        // { 
        //     MiniGameManager.Instance.CloseMiniGameView();   //미니게임 창 닫기
        // }
    }

    private void OnCollisionEnter(Collision collision) //건물 벽 등 맵 가장자리에 충돌
    {
        isCollided = true;
        // Debug.Log("Collided!");

        if (collision.gameObject.layer == 12)
        {
            // isColliding = true;
        }
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        // Debug.Log("Colliding!");
        isCollided = true;
    }









}
