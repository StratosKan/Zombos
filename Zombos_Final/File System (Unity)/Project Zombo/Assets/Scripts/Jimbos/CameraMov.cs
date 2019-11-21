using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    float yRotation;
    float xRotation;
    float lookSensitivity = 5;
    float currentXRotation;
    float currentYRotation;
    float yRotationVelocity;
    float xRotationVelocity;
    float lookSmoothnes = 0.1f;

    void Update()
    {
        yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
        xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -80, 100); // TO NOT LET it fully rotate on this Axis 
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationVelocity, lookSmoothnes); // This is for a smooth result while the transaction between the current postiion and the position we wanna reach
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationVelocity, lookSmoothnes); //  SAME
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
       

    }

}
