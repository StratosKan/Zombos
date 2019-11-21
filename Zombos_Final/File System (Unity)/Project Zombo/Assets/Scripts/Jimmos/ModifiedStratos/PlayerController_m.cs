using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //safety first 

public class PlayerController_m : MonoBehaviour
{
    // ****************************************************************************************
    // ***************** This PC is based on Jimmos' PlayerControllerScript *******************
    // ** It tries to remove player input from player object for more customization options. **
    // ****************************************************************************************

    private Rigidbody rb;

    private bool isGrounded;

    private string whatIsGround = "Floor";   //Every floor has the same tag - can be removed

    [SerializeField]
    private float movement_Speed = 1.0f;
    [SerializeField]
    private float sprint_Speed = 1.0f;
    [SerializeField]
    private float sprint_Multiplier = 2.0f;
    [SerializeField]
    private float jump_Force = 2.0f;
    //[SerializeField]
    //private float gameplay_Factor = 0.7f;      //thi

    private Vector2 localPIF;
    private Vector3 momentum;
    private Vector3 target_momentum;
    private Vector3 moveAmount;
    private Vector3 smoothMoveVelocity;

    private void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
    }


    public void Update() // Calculates in Update independetly from playerInputManager
    {
        momentum = new Vector3(localPIF.y, 0, localPIF.x);
        moveAmount = momentum * movement_Speed * sprint_Speed; //target_momentum = momentum * movement_Speed * sprint_Speed;
        //moveAmount = Vector3.SmoothDamp(moveAmount, target_momentum, ref smoothMoveVelocity, .05f,.25f,.5f);
        sprint_Speed = 1.0f;

        //rb.MovePosition(rb.position + rb.transform.TransformDirection(moveAmount)* Time.deltaTime); // Doesn't bond well with rigidbody
    }

    public void FixedUpdate()
    {
        rb.AddForce(transform.forward * moveAmount.z, ForceMode.Impulse);
        rb.AddForce(transform.right * moveAmount.x, ForceMode.Impulse);
        rb.AddForce(transform.up * (-350f * rb.mass)* Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    public void LateUpdate()
    {
        localPIF = Vector2.zero * Time.fixedDeltaTime;
    }

    public void Move(Vector2 playerInputForce)   // Receiving forces in a Vector2
    {
        localPIF = playerInputForce;
    }

    public void Jump()
    {
        rb.AddForce(transform.up * jump_Force, ForceMode.Impulse);
    }

    public void Sprint()
    {
        sprint_Speed = sprint_Multiplier;
    }

    public bool IsGrounded()
    {
        return isGrounded;
        //TODO: get; set;        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(whatIsGround))  //If the other gameObject the player hit isn't ground...
        {
            //Debug.Log("I hit floor tag");
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag(whatIsGround))  //If the other gameObject the player hit isn't ground...
        {
            //Debug.Log("Not on the ground");
            isGrounded = false;
        }
    }
}