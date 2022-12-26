using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using EnumTypes;
using UnityEditor.Searcher;


//클래스 이름: Player
//기능: 플레이어 캐릭터(Snowman)에게 붙는 스크립트.
//캐릭터의 조작, 애니메이션, 특정 범위 안에서의 상호작용 등을 관리한다.
public class Player : MonoBehaviour
{


    [HideInInspector] public Vector3 prevPos;
    [SerializeField] private int hMoveSpeed; //좌우 이동 속도
    [SerializeField] private Collider interactingObject; //상호작용한 오브젝트
    private bool _isCollided;
    private bool _canUsePassage = true;
    private bool _stopMove;
    private Vector2 _moveValue;
    private int _origSpeed;                   //처음 속도
    // private int _vMoveSpeed;                  //상하 이동 속도
    private SpriteRenderer _sprite;
    
    [HideInInspector] public Animator anim;
    private readonly int _isMoveId = Animator.StringToHash("isMove");
    private readonly int _isBackId = Animator.StringToHash("isBack");


    private void Awake()
    {
        anim = gameObject.GetComponentInChildren<Animator>();   //child의 animator 컴포넌트.
        _sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        _isCollided = false;
        hMoveSpeed = 4; // 가로 이동 속도 (translate)
        // hMoveSpeed = 30; // 가로 이동 속도 (doMove)
        // _vMoveSpeed = 100;
        _origSpeed = hMoveSpeed;
        _moveValue = Vector2.zero;
        prevPos = transform.position;
        GameManager.Input.keyAction += OnKeyboard;

    }

    private void FixedUpdate()
    {
        if (!IsMoving()) //이동 입력이 없으면 업데이트 하지 않음.
        {
            return;
        }
        if (!_isCollided) // 충돌하지 않을 때 이전 position 저장.
        {
            prevPos = transform.position;
        }
        MoveHorizontal(_moveValue.x); // 먼저 이동
    }

    private void LateUpdate()
    {
        if (!_isCollided)
        {
            return;
        }
        transform.position = prevPos; // 이전 위치로 이동
        _isCollided = false; // 충돌 처리 false
    }

    
    #region 퍼블릭 이동 조작
    
    
        //목표 지점으로 캐릭터를 움직인다. (컷신 용)
        public void MoveToDestination(Vector3 dest, float moveSpeed)
        {
            var distance = Vector3.Distance(transform.position, dest); //플레이어의 이동 거리
            anim.SetBool(_isMoveId, true);
            transform.DOMove(dest, distance / moveSpeed).SetEase(Ease.Linear); //속도 기반으로 목적지를 향해 움직이도록.
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
    private void InputMove()
    {
        if (GameManager.Instance.curGameFlowState == GameFlowState.Interacting || _stopMove)
        {
            return;
        }
        Debug.Log(_moveValue);

        if (_moveValue.y != 0) // 상하로 이동할 때
        {
            if (!interactingObject)
            {
                return;
            }

            if ( interactingObject.gameObject.layer == 10 && _canUsePassage ) // 트리거가 맵 이동과 연관되었다면?
            {
                _stopMove = true;
                var passage = interactingObject.gameObject.GetComponent<Passage>();
                switch (passage.moveType)
                {
                    case MoveType.Enter:
                        SetBackTrue();
                        break;
                    case MoveType.Exit:
                        SetBackFalse();
                        break;
                }
                MapManager.Instance.MoveToAnotherStreet(passage); // 거리 이동
                _moveValue = Vector2.zero;

                StartCoroutine(TimeWalkAnim(0.56f));
                StartCoroutine(CheckDelayTime());
                return;
            }
        }
        
        //애니메이션 parameter에 변화를 준다.
        if (_moveValue.x != 0)
        {
            StartWalkAnim();
        }
        else
        {
            StopWalkAnim();
        }
        
        _sprite.flipX = _moveValue.x switch //방향에 따라 스프라이트 반전
        {
            > 0 => false,
            < 0 => true,
            _ => _sprite.flipX
        };
    }
    
    //특정 행동을 무수히 반복하지 않도록 지연을 준다.
    private IEnumerator CheckDelayTime()
    {
        _canUsePassage = false;
        yield return new WaitForSeconds(0.7f);
        _canUsePassage = true;
        yield return null;
    }

    
    // 이름 : MoveHorizontal
    // 기능 : 인자로 받는 xValue만큼 X 이동
    private void MoveHorizontal(float xValue)   //moveValue.x -> position.x
    {
        transform.Translate(new Vector3
            (xValue * hMoveSpeed * Time.fixedDeltaTime, 0, 0));
    }

    
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
            _stopMove = false;
            StopWalkAnim();
            SetBackFalse();
        }
        

    #endregion
    
   
    
    // 이름 : IsMoving
    // 기능 : moveValue가 0인지 체크하고, bool 반환
    private bool IsMoving() => _moveValue != Vector2.zero;
    
    //interactingObject를 지운다.
    public void EraseInteractingObject() => interactingObject = null;

    //해당 시간동안 스피드를 0으로 만든다. 점프할 시 속도 조정 위해
    private IEnumerator StopSpeedForTime(float time)
    {
        hMoveSpeed = 0;
        yield return new WaitForSeconds(time);
        hMoveSpeed = _origSpeed + 4;
        yield return new WaitForSeconds(time * 2);
        hMoveSpeed = _origSpeed;
        yield return null;
    }


    private void OnTriggerEnter(Collider interacted)
    {
        interactingObject = interacted; //접촉한 콜라이더가 존재할 시
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

    // 트리거에서 빠져나올 시 UIManager, CameraController가 가지고 있던 창, 뷰 원위치
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
        
        //점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StopSpeedForTime(0.2f));
            anim.Play("Jump");
        }
        
        //좌우 이동
        if (Input.GetKey(KeyCode.A))
        {
            _moveValue.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _moveValue.x = 1;
        }
        else if(Input.GetKeyUp(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            Debug.Log("GetKeyUp");
            _moveValue.x = 0;
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
                anim.SetBool("isMove", false);
                GameManager.Instance.curGameFlowState = GameFlowState.Interacting; //게임 상태 상호작용으로 바꿈
                GameManager.Instance.isInteracted = true; // 상호작용 시 대화창 읽기 바로 시작 안 되도록
                CharacterManager.Instance.curInteractingVillager = interacting;
                interacting.Interact(); //Villager/Item.cs 의 함수
                CameraController.Instance.SaveZoomRange(0.2f);
                UIManager.Instance.StartCoroutine( "OpenDialoguePopup" );
            }
            _moveValue = Vector2.zero;
        }
        
        

    }









}
