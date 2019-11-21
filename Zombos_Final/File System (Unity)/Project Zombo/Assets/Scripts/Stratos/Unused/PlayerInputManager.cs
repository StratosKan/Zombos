using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    //This player input manager tries to add an extra layer between character movement and player input.

    private PlayerController playerController;

    private bool PLAYING_STATE = true; //Fake state machine.
    private bool PAUSE_STATE = false;

    private Vector3 defaultForce;

    [SerializeField]
    private float movementSpeed = 10.0f;          
    
    [SerializeField]
    private float default_Y = -5.0f;
    
    // W.I.P.
    //private float inputPower = 1.0f;       
    //public bool boostPickedUp;             //This is to let booster, boost.
    //private float boostDisplayTime;   
    //private float gameStartMessagesTimer = 32f;
    //private bool isTutorialActive = true;
    //private bool cheatSpeedMultiplier = false;

    void Start ()
    {
        //setting up refs
        //this.playerController = this.GetComponent<PlayerController>();
        this.playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Debug.Log("Found " + playerController.ToString());

        this.defaultForce = Vector3.zero;  //(0,0,0)
        this.defaultForce.y = default_Y;   //(0,-5,0)
  	}

    void FixedUpdate()
    {
        if (PLAYING_STATE)
        {
            Movement();  // Checks and operates commands related to player movement.

            Options();

            //Abilities();
            //Interractions();
        }
        else if (PAUSE_STATE) // NOT IMPLEMENTED YET
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                QuitGame();
            }
        }        
    }

    Vector2 GetInput()
    {
        return new Vector2 //making vector2 
        {
            x = Input.GetAxis("Vertical") * movementSpeed,
            y = Input.GetAxis("Horizontal") * movementSpeed
        };
    }
    
    public void Movement ()
    {
        Vector3 forceInput = new Vector3(GetInput().y, default_Y, GetInput().x);  //we make certain this value runs only for this method.

         //default_Y = -5.0f; //Jimmos' safety on jump

        if (Input.GetKey(KeyCode.Space))
        {
              if (playerController.IsGrounded())
              {
                playerController.Jump(forceInput);
                //default_Y += jump_Y;
              }
        }                        

        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            playerController.Sprint();      //enables the sprint factor on P.C. script.
            //TODO: stamina drain ?
        }

        if (forceInput != defaultForce)         // ...checking if input is null
        {
            playerController.Move(forceInput);  // ...and sending it to player.
            //Debug.Log("Sending " + forceInput);
        }
    }

    public void Options()             
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            QuitGame(); // Later version will have a way to pause instead of quit.
        }
    }
    
    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    //public void OnGUI()
    //{
    //    if (isTutorialActive)
    //    {
    //        if (gameStartMessagesTimer >= 24)
    //        {
    //            GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, 200f, 200f), ""); 
    //            gameStartMessagesTimer -= Time.deltaTime;
    //        }
    //        else if (gameStartMessagesTimer >= 16)
    //        {
    //            GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, 200f, 200f), ""); 
    //            gameStartMessagesTimer -= Time.deltaTime;
    //        }
    //        else if (gameStartMessagesTimer >= 8)
    //        {
    //            GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, 200f, 200f), ""); 
    //            gameStartMessagesTimer -= Time.deltaTime;
    //        }
    //        else if (gameStartMessagesTimer > 0)
    //        {
    //            GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, 200f, 200f), ""); 
    //            gameStartMessagesTimer -= Time.deltaTime;
    //        }
    //        else if (gameStartMessagesTimer <= 0)
    //        {
    //            isTutorialActive = false;
    //        }
    //    }
    //    else if (boostPickedUp && boostDisplayTime >= 0)
    //    {
    //        GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, 150f, 150f), ""); 

    //        boostDisplayTime -= Time.deltaTime;
    //    }
    //}
}
