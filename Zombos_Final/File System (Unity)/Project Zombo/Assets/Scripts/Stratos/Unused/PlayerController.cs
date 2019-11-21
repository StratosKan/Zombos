using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //safety first 

public class PlayerController : MonoBehaviour
{
    // ****************************************************************************************
    // ***************** This PC is based on Jimmos' PlayerControllerScript *******************
    // ** It tries to remove player input from player object for more customization options. **
    // ****************************************************************************************

    private Rigidbody rb;

    private bool isGrounded;

    private string whatIsGround = "Floor";   //Every floor has the same tag - can be removed

    [SerializeField]
    private float sprint_Multiplier = 1.0f;
    [SerializeField]
    private float sprint_Multiplier_Active = 2.0f;
    [SerializeField]
    private float gameplay_Factor = 0.7f;      //thi

    private void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
    }

    public void Move(Vector3 playerInputForce)                                // Receiving forces in a Vector3
    {
            Vector3 momentum = playerInputForce;
            momentum = this.transform.rotation * momentum;
            rb.velocity = momentum * sprint_Multiplier * gameplay_Factor;
            sprint_Multiplier = 1.0f;           
    }
    public void Jump(Vector3 playerInputForce)
    {
        Vector3 momentum = this.transform.rotation * new Vector3(playerInputForce.x * 2, 15, playerInputForce.z * 2);
        this.rb.velocity = momentum;

        //this.rb.AddForce(0, 1000, 0, ForceMode.Acceleration);
    }
    public void Sprint()
    {
        sprint_Multiplier = sprint_Multiplier_Active;
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
    

