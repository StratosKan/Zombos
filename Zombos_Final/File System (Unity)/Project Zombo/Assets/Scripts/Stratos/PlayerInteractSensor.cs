using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractSensor : MonoBehaviour
{
    public Transform pointer; //Setting gun camera as pointer 
    public float interactMaxDistance;
    private Vector3 forward;

    private readonly string interactableTag = "Interactable";
    private bool displayingInteraction;
    private RaycastHit hit;

    private InputManager inputManager;
    private UI_Manager uiManager;
    
	void Start ()
    {
        this.inputManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>();
        this.uiManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UI_Manager>();
	}
	
	void Update ()
    {
        forward = this.pointer.TransformDirection(Vector3.forward);

        if (Physics.Raycast(this.pointer.position,forward,out hit,interactMaxDistance))    //Casting the ray...
        {
            if (hit.transform.CompareTag(interactableTag))                               //...check 1 (tag)
            {
                displayingInteraction = true;
                uiManager.UpdateInteractionInUI(displayingInteraction);                 //...displaying Interact Button

                if (inputManager.Interaction)                                          //... reads if player presses Interact Button
                {
                    if(Physics.Linecast(this.pointer.position, hit.transform.position))  //... check 2 (linecast)
                    {
                        hit.transform.GetComponent<IInteractable>().Interact();          //...magic
                    }
                }
                //Debug.Log("Can Interact "+Time.deltaTime);
            }
        }
        else
        {
            if (displayingInteraction)
            {
                displayingInteraction = false;
                uiManager.UpdateInteractionInUI(displayingInteraction);
            }
            //Debug.Log("Can't Interact "+Time.deltaTime);
        }
    }
}
