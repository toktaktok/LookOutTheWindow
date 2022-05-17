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
    }

    private void Update()
    {
        if (moveValue == Vector2.zero)
            anim.SetBool("isMove", false);
    }

    //플레이어 이동 계산 
    public void FixedUpdate()
    {
        transform.Translate(new Vector3(moveValue.x, 0, moveValue.y) * moveSpeed * Time.deltaTime);
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();        
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
     * 기능 : InputAction을 통해 상효작용 할 시(input E) 미니게임이 있는지 확인한다.
     */
    public void Interact(InputAction.CallbackContext ctx)
    {
        if(interactingObject != null)
        {
            IsHaveMinigame(interactingObject);
        }
    }



    void OnTriggerEnter(Collider interacted)
    {
        UIManager.Instance.OpenInteractionUI();
        interactingObject = interacted;
        //isStaying = true;

    }


    void OnTriggerExit(Collider interacted)
    {
        UIManager.Instance.CloseInteractionUI();
        //isStaying = false;
        interactingObject = null;


        //CameraController.Instance.CloseMinigameView();
        UIManager.Instance.CloseMinigameView();
        CameraController.Instance.ReturnMinigameView();
    }


    void IsHaveMinigame(Collider interacted)
    {
        interacted.SendMessage("Check");
    }







}
