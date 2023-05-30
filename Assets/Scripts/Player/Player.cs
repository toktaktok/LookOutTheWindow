using System.Collections;
using UnityEngine;
using DG.Tweening;
using EnumTypes;
using static GlobalVariables.LayerNumber;

//클래스 이름: Player
//기능: 플레이어 캐릭터(Snowman)에게 붙는 스크립트.
//캐릭터의 조작, 애니메이션, 특정 범위 안에서의 상호작용 등을 관리한다.
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMoveController moveController;
    [SerializeField] private float hMoveSpeed;                  //좌우 이동 속도
    [SerializeField] private Collider interactingObject;        //상호작용하는 오브젝트 콜라이더
    private bool _isCollided;                                   //충돌했는가?
    private bool _canUsePassage = true;                         //통로로 이동할 수 있는가?
    private bool _canJump = true;                               //점프할 수 있는가?
    private bool _stopMove;                                     //캐릭터 조작 멈추기
    private Vector2 _moveValue;
    private Vector3 _prevPos;                                   //이동 가능 시 이전 위치 저장
    private float _origSpeed;                                   //처음 속도
    // private int _vMoveSpeed;                                 //상하 이동 속도
    private SpriteRenderer _sprite;
    private CharacterController _characterController;           //플레이어의 characterController
    private Vector2 lastMoveDir;
    [SerializeField] private Vector2 moveDir;                   //이동 방향
    private Direction _direction;
    
    [HideInInspector] public Animator anim;
    private readonly int _isMoveId = Animator.StringToHash("isMove");
    private readonly int _isBackId = Animator.StringToHash("isBack");

    public Collider InteractingObject
    {
        set
        {
            interactingObject = value;
            if (value != null)
            {
                SwitchInteractingObject();
            }
            else
            {
                FinishInteracting();
            }
        }
    }

    public Direction CurrentDirection
    {
        set
        {
            _direction = value;
            SwitchDirection();
        }
    }

    private void Awake()
    {
        anim = gameObject.GetComponentInChildren<Animator>();   //child의 animator 컴포넌트.
        _sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        _characterController = gameObject.GetComponent<CharacterController>();
    }
    
    private void Start()
    {
        GameManager.Input.keyAction += OnKeyboard;
        hMoveSpeed = 5f;                // 가로 이동 속도 (translate)
        // hMoveSpeed = 30;             // 가로 이동 속도 (doMove)
        _origSpeed = hMoveSpeed;
        _prevPos = transform.position;
        _isCollided = false;
    }
    
    private void Update()
    {
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) //A,D 키를 누르면
        {

            _sprite.flipX = moveDir.x switch //방향에 따라 스프라이트 반전
            {
                > 0 => false,
                < 0 => true,
                _ => _sprite.flipX
            };
            
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                StartWalkAnim();
            }
        }
        else
        {
            StopWalkAnim();
            moveDir = Vector2.zero;
            return;
        }
        
        if (GameManager.Instance.curGameFlowState == GameFlowState.Interacting || _stopMove)
        {
            return;
        }
        
        if (!_isCollided) // 충돌하지 않을 때 이전 position 저장.
        {
            _prevPos = transform.position;
        }

        MoveHorizontal( hMoveSpeed * Time.deltaTime * moveDir ); // 먼저 이동
    }

    private void SwitchDirection()
    {
        _sprite.flipX = _direction switch //방향에 따라 스프라이트 반전
        {
            Direction.Left => false,
            Direction.Right => true,
            _ => _sprite.flipX
        };
    }

    private void SwitchInteractingObject()
    {
        var curGameObject = interactingObject.gameObject;
        
        switch (curGameObject.layer)
        {
            case character:                 //캐릭터 트리거
                UIManager.Instance.OpenInteractionKey();                //Trigger와 접촉할 시 상호작용 키를 보이게 한다.
                break;
            case map:                       //Map 관련 트리거
                UIManager.Instance.OpenMapMovingUI();
                break;
        }
    }

    private void FinishInteracting()
    {
        switch (interactingObject.gameObject.layer)
        {
            case character:
                UIManager.Instance.CloseInteractionKey();       //상호작용 UI 닫기
                break;
            case map:               //Map 관련 트리거 진입
                UIManager.Instance.CloseMapMovingUI();
                break;
        }
    }
    
    private void LateUpdate() //충돌 전 position 갱신
    {
        if (!_isCollided)
        {
            return;
        }
        transform.position = _prevPos; // 이전 위치로 이동
        _isCollided = false; // 충돌 처리 false
    }

    
    #region 퍼블릭 이동 조작
    
        //목표 지점으로 캐릭터를 움직인다. (컷신 용)
        public void MoveToDestLinear(Vector3 dest, float moveSpeed)
        {
            var distance = Vector3.Distance(transform.position, dest); //플레이어의 이동 거리
            anim.SetBool(_isMoveId, true);
            transform.DOMove(dest, distance / moveSpeed).SetEase(Ease.Linear); //속도 기반으로 목적지를 향해 움직이도록.
        }
        //목표 지점으로 즉시 이동한다.
        public void MoveToDestInstant(Transform pos)
        {
            transform.SetPositionAndRotation(pos.position, pos.rotation);
        }
        
        //이동속도를 바꾼다.bool로 조정. 이동을 제한해야 할 경우 0으로 만듬
        public void SwitchSpeed(bool isStop)
        {
            hMoveSpeed = isStop switch
            {
                true => 0,
                false => _origSpeed
            };
        }
        
        //특정 거리로 이동
        public void MoveStreet(Vector3 pos)
        {
            transform.DOMove(new Vector3(transform.position.x, pos.y, pos.z), 0.7f).SetEase(Ease.Linear);
            // StartCoroutine(TimeWalkAnim(0.8f));
        }
        
    #endregion

    
    //moveValue 맞춰 애니메이션 재생
    // private void SwitchDir()
    // {
    //     //애니메이션 parameter에 변화를 준다.
    //
    //     // _sprite.flipX = moveDir.x switch //방향에 따라 스프라이트 반전
    //     // {
    //     //     > 0 => false,
    //     //     < 0 => true,
    //     //     _ => _sprite.flipX
    //     // };
    // }
    
    //특정 행동을 무수히 반복하지 않도록 지연을 준다.
    
    
    private IEnumerator CheckDelayTime(float time)
    {
        _canUsePassage = false;
        yield return new WaitForSeconds(time);
        _canUsePassage = true;
        yield return null;
    }

    
    // 이름 : MoveHorizontal
    // 기능 : 인자로 받는 xValue만큼 X 이동
    //moveValue.x -> position.x
    private void MoveHorizontal(Vector2 value) => _characterController.Move(new Vector3(value.x, 0, value.y));
    
    
    #region 애니메이션 설정
    
        // 기능 : 애니메이션의 isMove 프로퍼티 T/F set
        public void StartWalkAnim() => anim.SetBool(_isMoveId, true);
        public void StopWalkAnim() => anim.SetBool(_isMoveId, false);
        private void SetBackTrue() => anim.SetBool(_isBackId, true);
        private void SetBackFalse() => anim.SetBool(_isBackId, false);
        
        //애니메이션의 isIntro 프로퍼티 true
        public void SetIntroAnim() => anim.SetBool("isIntro", true);
        
        
        private IEnumerator TimeWalkAnim(float playTime) //특정 시간동안 애니메이션 재생 위해
        {
            _stopMove = true;
            StartWalkAnim();
            yield return new WaitForSeconds(playTime);
            
            SetBackFalse();
            _stopMove = false;

            if (moveDir.x != 0)
            {
                StartWalkAnim();
            }
            else
            {
                StopWalkAnim();
            }
        }
        

    #endregion
    
    // 이름 : IsMoving
    // 기능 : moveValue가 0인지 체크하고 bool 반환
    private bool IsMoving() => moveDir != Vector2.zero;
    
    
    //해당 시간동안 스피드를 0으로 만든다. 점프할 시 속도 조정 위해
    // private IEnumerator Jump(float time)
    // {
    //     _canJump = false;
    //     hMoveSpeed = 0;
    //     yield return new WaitForSeconds(time);
    //     hMoveSpeed = _origSpeed + 2f;
    //     yield return new WaitForSeconds(time * 2);
    //     hMoveSpeed = _origSpeed;
    //     yield return new WaitForSeconds(time * 2);
    //     _canJump = true;
    //     yield return null;
    // }


    //상호작용 할 수 있는 트리거 진입
    private void OnTriggerEnter(Collider interacted)
    {
        interactingObject = interacted; //접촉한 콜라이더가 존재할 시
        switch (interacted.gameObject.layer)
        {
            case GlobalVariables.LayerNumber.character:                 //캐릭터 트리거
                UIManager.Instance.OpenInteractionKey();                //Trigger와 접촉할 시 상호작용 키를 보이게 한다.
                break;
            case GlobalVariables.LayerNumber.map:                       //Map 관련 트리거
                UIManager.Instance.OpenMapMovingUI();
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

    // 트리거에서 빠져나올 시 UIManager, CameraController가 가지고 있던 창, 뷰 원위치
    private void OnTriggerExit(Collider interacted)
    {

        // CameraController.Instance.ReturnInteractionView();  //카메라 줌 수치를 상호작용 전 시점으로 돌린다.
        
        //미니게임이 실행중이었다면?
        // if (MiniGameManager.Instance.IsMiniGamePlaying())
        // { 
        //     MiniGameManager.Instance.CloseMiniGameView();   //미니게임 창 닫기
        // }
    }

    private void OnCollisionEnter(Collision collision) //건물 벽 등 맵 가장자리에 충돌 시
    {
        _isCollided = true;
        if (collision.gameObject.layer == 12)
        {
            //isColliding = true;
        }
    }

    private void OnCollisionStay()
    {
        _isCollided = true;
    }

    //키보드 조작
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
                _stopMove = true;
                var walkAnimTime = 0.7f;
                switch (interactingObject.tag)
                {
                    case "Passage":
                        Passage passage1 = interactingObject.gameObject.GetComponent<Passage>();
                        switch (passage1.moveType)
                        {
                            case MoveType.Enter:
                                SetBackTrue();
                                break;
                            case MoveType.Exit:
                                SetBackFalse();
                                break;
                        }
                        MapManager.Instance.MoveToAnotherStreet(passage1); // 거리 이동
                        StartCoroutine(TimeWalkAnim(walkAnimTime));

                        break;
                    case "BuildingDoor":
                        BuildingPassage passage2 = interactingObject.gameObject.GetComponent<BuildingPassage>();
                        MapManager.Instance.EnterBuilding(passage2.exitPos);
                        
                        
                        break;
                }

                moveDir = Vector3.zero;
                _stopMove = false;
                StartCoroutine(CheckDelayTime(walkAnimTime));
                return;
            }
        }
        
        //점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_canJump)
            {
                StartCoroutine(moveController.Jump(0.2f));
                anim.Play("Jump");
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
                StopWalkAnim();
                GameManager.Instance.curGameFlowState = GameFlowState.Interacting; //게임 상태 상호작용으로 바꿈
                GameManager.Instance.isInteracted = true; // 상호작용 시 대화창 읽기 바로 시작 안 되도록
                CharacterManager.Instance.curInteractingVillager = interacting;
                interacting.Interact(); //Villager/Item.cs 의 함수
                UIManager.Instance.StartCoroutine( "OpenDialoguePopup" );
            }
            moveDir = Vector3.zero;
            // _moveValue = Vector2.zero;
        }
    }









}
