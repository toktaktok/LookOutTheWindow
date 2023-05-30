using System;
using UnityEngine;
using EnumTypes;
using DG.Tweening;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class PlayerMoveController : MonoBehaviour
{
    
    private const float OrigSpeed = 5f;                         //처음 속도
    [SerializeField] private float hMoveSpeed;                  //좌우 이동 속도
    [SerializeField] private Collider interactingObject;        //상호작용하는 오브젝트의 콜라이더

    // private bool _isCollided;                                   //충돌했는가?
    private bool _canUsePassage = true;                         //통로 사용 가능한가?
    private bool _canJump = true;                               //점프할 수 있는가?
    // private bool _stopMove;                                     //캐릭터 조작 멈춤
    // private Vector2 _moveValue;
    private Vector3 _prevPos;                                   //이동 가능 시 이전 위치 저장
    [SerializeField] private Vector2 moveDir;
    private Vector2 _lastMoveDir;                                //이동 방향
    private CharacterController _characterController;           //플레이어의 characterController
    private Player _player;
    
    private void Awake()
    {
        _player = gameObject.GetComponent<Player>();
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    private void Start()
    {
        hMoveSpeed = OrigSpeed;
    }

    private void Update()
    {
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) //A,D 키를 누르면
        {
            var hor = Input.GetAxis("Horizontal");                //수평 좌표 값 받기
            moveDir = new Vector2(hor,0).normalized;
            MoveHorizontal( hMoveSpeed * Time.deltaTime * moveDir);
            _lastMoveDir = moveDir;
        }
        
    }
    
    #region 퍼블릭 이동 조작 
    public void Stop() => hMoveSpeed = 0;

    public void Move() => hMoveSpeed = OrigSpeed;
    
    public void MoveHorizontal (Vector2 value) => _characterController.Move(new Vector3(value.x, 0, value.y));

    //맵 이동
    public void UsePassage(Vector3 destPos)
    {
        var vertPos = new Vector3(transform.position.x, destPos.y, destPos.z);
        MoveToDestLinear(destPos);
    }
    
    //목표로 보간 이동
    public void MoveToDestLinear(Vector3 dest)
    {
        var distance = Vector3.Distance(transform.position, dest);
        transform.DOMove(dest, distance /OrigSpeed ).SetEase(Ease.Linear);
    }
    
    //목표로 즉시 이동
    public void MoveToDestInstant(Transform pos)
    {
        transform.SetPositionAndRotation(pos.position, pos.rotation);
    }
    
    #endregion
  

    private IEnumerator Jump(float time)
    {
        _canJump = false;
        hMoveSpeed = 0;
        yield return new WaitForSeconds(time);
        hMoveSpeed = OrigSpeed + 2f;
        yield return new WaitForSeconds(time * 2);
        hMoveSpeed = OrigSpeed;
        yield return new WaitForSeconds(time * 2);
        _canJump = true;
        yield return null;
    }


    private void SetInteractingCollider(Collider interacted)
    {
        interactingObject = interacted;
        _player.InteractingObject = interacted;
    }

    //parameter로 받은 시간동안 캐릭터 조작을 멈춘다.
    private IEnumerator RestrictControlForTime(float playTime) 
    {
        Stop();
        yield return new WaitForSeconds(playTime);
        Move();
    }
    
    /*
     기능: 받은 시간동안 맵 이동을 제한한다.
    */
    private IEnumerator RestrictUsingPassageForTime(float time)
    {
        _canUsePassage = false;
        yield return new WaitForSeconds(time);
        _canUsePassage = true;
    }

    
    private void OnTriggerEnter(Collider collider)
    {
        SetInteractingCollider(collider);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (interactingObject)
        {
            return;
        }
        SetInteractingCollider(collider);
    }

    // 트리거에서 빠져나올 시 UIManager, CameraController가 가지고 있던 창, 뷰 원위치
    private void OnTriggerExit(Collider collider)
    {
        interactingObject = null;
        _player.InteractingObject = null;
        switch (collider.gameObject.layer)
        {
            case GlobalVariables.LayerNumber.character:
                UIManager.Instance.CloseInteractionKey();       //상호작용 UI 닫기
                break;
            case GlobalVariables.LayerNumber.map:               //Map 관련 트리거 진입
                UIManager.Instance.CloseMapMovingUI();
                break;
        }
        
        // CameraController.Instance.ReturnInteractionView();  //카메라 줌 수치를 상호작용 전 시점으로 돌린다.
        
        //미니게임이 실행중이었다면?
        // if (MiniGameManager.Instance.IsMiniGamePlaying())
        // { 
        //     MiniGameManager.Instance.CloseMiniGameView();   //미니게임 창 닫기
        // }
    }

    private void OnCollisionEnter(Collision collision) //건물 벽 등 맵 가장자리에 충돌 시
    {
        // _isCollided = true;
        if (collision.gameObject.layer == 12)
        {
            //isColliding = true;
        }
    }

    private void OnCollisionStay()
    {
        // _isCollided = true;
    }
    
    private void OnKeyboard()
    {
        //E를 눌렀을 시 interactingObject가 존재하거나, 현재 인게임 상태가 아니라면 실행하지 않음
        if (GameManager.Instance.curGameFlowState != GameFlowState.InGame)
        {
            return;
        }
        
        //방향키 위 버튼
        if (Input.GetKeyDown(KeyCode.W)) //이동 입력이 없으면 업데이트 하지 않음.
        {
            // vertDir = 1;
            
            if (!interactingObject)
            {
                // vertDir = 0;
                return;
            }

            //통로 이동
            if ( interactingObject.gameObject.layer == 10 && _canUsePassage ) // 트리거가 맵 이동과 연관되었다면?
            {
                Stop();
                var walkAnimTime = 0.7f;
                switch (interactingObject.tag)
                {
                    case "Passage":
                        Passage passage1 = interactingObject.gameObject.GetComponent<Passage>();
                        MapManager.Instance.MoveToAnotherStreet(passage1); // 거리 이동
                        StartCoroutine(RestrictControlForTime(walkAnimTime));

                        break;
                    case "BuildingDoor":
                        BuildingPassage passage2 = interactingObject.gameObject.GetComponent<BuildingPassage>();
                        MapManager.Instance.EnterBuilding(passage2.exitPos);
                        
                        
                        break;
                }

                moveDir = Vector3.zero;
                Move();
                StartCoroutine(RestrictUsingPassageForTime(walkAnimTime));
                return;
            }
        }
        
        //점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_canJump)
            {
                StartCoroutine(Jump(0.2f));
            }
        }
        
        
        // 상호작용
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!interactingObject)
            {
                return;
            }
            if (interactingObject.TryGetComponent(out Villager interacting)) //상호작용 성공했을 시
            {
                CameraManager.Instance.ModifyZoomRange(2);
                Stop();
                interacting.Interact();
            }
            moveDir = Vector3.zero;
        }
    }
}
