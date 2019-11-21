using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    //Attempt to add movement to the Player using the Character Controller component of unity

    public float walkSpeed = 4.0F;
    public float jumpSpeed = 8.0F;
    public float runSpeed = 8.0F; //it can be use as a sprint fuction in the near future
    public float gravity = 20.0F;
    public Transform CamTransform { set; get; } //  Take the properties of the Camera
   







    private Vector3 moveDirection = Vector3.zero; 
    private CharacterController controller; 

    void Start()
    {
        controller = GetComponent<CharacterController>(); // //Accessing the Character controller component
        CamTransform = Camera.main.transform; // We want the position and the rotation 
       
    }

    void Update()
    {
        if (controller.isGrounded) // Checking if our player touches the ground( The reason i chose Character controller component was mainly this one part)
        {
                 

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 dir = CamTransform.TransformDirection(moveDirection);

            moveDirection = transform.TransformDirection(dir.x, 0, dir.z); /* So here we are, this code line here is an attempt to move player where camera is looking BUT at first when 
                                                                          u looked up it was trying to move the player in the Y axis */
           
            moveDirection *= walkSpeed;
             if (Input.GetButton("Jump"))
                  moveDirection.y = jumpSpeed; // If space is pressed then the player jumps accordingly( maintains his velocity)
            
           
        }
        



        moveDirection.y -= gravity * Time.deltaTime; // Implementation of gravity 
        controller.Move(moveDirection * Time.deltaTime); // Making that little boy move
       
    }
}
