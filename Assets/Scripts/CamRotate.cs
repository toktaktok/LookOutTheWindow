using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamRotate : MonoBehaviour
{

    public Quaternion origRotation;
    public Quaternion targetRotation;

    public float rotateValue;
    Quaternion plusRotation;

    void Start()
    {
        rotateValue = 0;
        origRotation = transform.rotation;
    }

    public void Rotate(InputAction.CallbackContext ctx)
    {

        rotateValue += ctx.ReadValue<float>();
        

        if (rotateValue < -11)
            rotateValue = -10;
        else if (rotateValue > 11)
            rotateValue = 10;
        plusRotation = Quaternion.Euler(new Vector3(0, rotateValue, 0));


        targetRotation = origRotation * plusRotation;
    }

    void Update()
    {
        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            10.0f * Time.deltaTime);
    }
}
