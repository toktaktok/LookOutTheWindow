using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamRotate : MonoBehaviour
{

    public Quaternion origRotation;     //저장된 회전 값
    public Quaternion targetRotation;   //바뀔 회전 값

    public float rotateValue;
    Quaternion plusRotation;

    private void Start()
    {
        rotateValue = 0;
        origRotation = transform.rotation;
    }

    public void Rotate(InputAction.CallbackContext ctx)
    {
        rotateValue += ctx.ReadValue<float>();
        rotateValue = rotateValue switch
        {
            < -6 => -5,
            > 6 => 5,
            _ => rotateValue
        };

        plusRotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));


        // targetRotation = origRotation * plusRotation;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, (origRotation * plusRotation),
            10.0f * Time.deltaTime);
    }
}
