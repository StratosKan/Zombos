using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWith : MonoBehaviour, IInteractable
{
    [Header("Implemented behaviours: SpinningCube , StimPack , AmmoPack , Door , Lever")]
    public string whatAmI;
    private bool shouldInteract = true;
    private bool shouldSpin = false;
    private float speed = 625f;

    public void Interact()
    {
        if (shouldInteract)
        {
            Debug.Log("Interacting with " + this.transform.name);

            switch (whatAmI)
            {
                case "SpinningCube": Spin();  break;
                case "StimPack": StimPack(); break;
                case "AmmoPack": AmmoPack(); break;
                case "Door": DoorInteraction(); break;
                case "Lever": LeverInteraction(); break;
            }
        }
    }

	private void Update ()
    {
        if (shouldSpin)
        {
            this.transform.Rotate(Vector3.up, speed * Time.deltaTime);
        }
	}

    public void Spin()
    {
        shouldSpin = true;
    }

    public void StimPack()
    {
        PlayerHealth pHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>(); //hardcoded<3
        pHealth.AddStimPack();
        this.transform.gameObject.SetActive(false);
    }

    public void AmmoPack()
    {
        // _@_ //
        //(-.-)//
        //_| |_//
    }

    public void DoorInteraction()
    {
        //either reference to another script or implement here.
    }
    public void LeverInteraction()
    {
        //same as door
    }
}
