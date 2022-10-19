using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    //델리게이트
    public Action keyAction = null;

    public void OnUpdate()
    {
        //키 입력 없을 시 종료
        if (Input.anyKey == false)
        {
            return;
        }

        //키입력 존재한다면 keyAction에서 이벤트 발생 알림
        if (keyAction != null)
        {
            keyAction.Invoke();
        }
    }
}
