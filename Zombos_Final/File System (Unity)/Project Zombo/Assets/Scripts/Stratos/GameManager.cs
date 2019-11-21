using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//v3
[RequireComponent(typeof(Stats_Manager))]
[RequireComponent(typeof(AI_Manager))]
public class GameManager : MonoBehaviour
{
    //Game Design changed as of 01/03/19 so game is now survival with waves of enemies (Old code is commented for future use).

    //private List<GameEventListener> listeners = new List<GameEventListener>(); //Creates a new list of game event listeners.
    //private readonly string[] gameEventNames = new string[7] {"OnNewGame","OnParking","OnCollegeArea","OnSecondFloor", "OnSecretLab", "OnSecretLabTwo","OnEndlessArcadeMode"};
    //private bool[] gameEventNotUsed = new bool[7] { true, true, true, true, true, true, true };
    //private string currentObjective;
    //private int hiddenScore = 0;

    private Stats_Manager statsManager;
    private AI_Manager aiManager;

    //private string currentDifficulty;  //Singleton Class provides this info.
    private bool currentDifficultyBool; //to communicate with other systems.

    private int currentWave = 0;
    private readonly int MAX_ALLOWED_WAVES = 20;
    private string currentState;

    private float safetyWaveTimer;
    private float defaultSafetyWaveTimer = 120f; //2 minutes max round duration (max Game duration 40 minutes)
    private bool waveSpawning;

    public void Start ()
    {

        this.statsManager = this.GetComponent<Stats_Manager>();
        this.aiManager = this.GetComponent<AI_Manager>();
        currentState = statsManager.GetGameState();

        currentDifficultyBool = GameObject.Find("PersistentGameObject").GetComponent<OurSingletonFriend>().GetDifficulty();
        //this.SetDifficulty(currentDifficultyBool);  //We get the difficulty from our persistent game object that contains a singleton script.
        aiManager.SetDifficulty(currentDifficultyBool);

        if (currentState == null)
        {
            Debug.Log("ERROR: FAILED TO LOAD CURRENTLY ACTIVE STATE");
        }

        NextWave(); //check execution order
    }
    public void Update()
    {
        if(statsManager.GetPlayerHealth() <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //DED SCENE
            SceneManager.LoadScene(2);
        }

        if (currentWave == MAX_ALLOWED_WAVES)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //WINNER SCENE
            SceneManager.LoadScene(3);
        }

        if (waveSpawning)
        {
            safetyWaveTimer -= Time.deltaTime;
            if(safetyWaveTimer <= 0)
            {
                waveSpawning = false;
                NextWave();
            }
        }
    }

    public void NextWave()
    {
        safetyWaveTimer = defaultSafetyWaveTimer; //Making sure game goes on, no matter how many stuck zombos xD
        waveSpawning = true;
       
        currentWave++;

        statsManager.SetWave(currentWave); //8 SECONDS CROSSFADE
        StartCoroutine("WaveCooldown");
    }
    IEnumerator WaveCooldown()
    {
        yield return new WaitForSeconds(7.0f);
        aiManager.SpawnNextWave(currentDifficultyBool);
    }

    //public void SetDifficulty(bool difficulty) //true for hard , false for normal 
    //{
    //    if (difficulty)
    //    {
    //        currentDifficulty = "HARD";
    //        Debug.Log("GAME_MANAGER: Setting Difficulty to HARD.");
    //    }
    //    else
    //    {
    //        currentDifficulty = "NORMAL";
    //        Debug.Log("GAME_MANAGER: Setting Difficulty to NORMAL.");
    //    }
    //}

    //public void Update()
    //{
    //    if (hiddenScore >= 150) //+10 on zombo kill
    //    {
    //        Debug.Log("Hidden Score is 150. Initiating special event (maybe play some hidden audio clip?)");
    //    }
    //    else if (hiddenScore >= 80)
    //    {
    //        Debug.Log("Hidden Score is 80. Initiating special event (maybe play some hidden audio clip?)");
    //    }
    //}

    //public void AddToHiddenScore(int amount)
    //{
    //    hiddenScore += amount;
    //}

    //DEPRECATED AS OF 01/03/19 (Works fine)
    //public static GameManager GetGameManager()
    //{
    //    return inst;
    //}
    //public void ChangeObjectiveDisplay(string newObjective)
    //{
    //    this.currentObjective = newObjective;
    //    //TODO: Reference to text on screen + fade effect.
    //    //this.statsManager.ChangeActiveObjective(newObjective);
    //}
    public void ChangeGameState(string newStateName)
    {
        this.currentState = newStateName;
        this.statsManager.ChangeGameState(newStateName);
        //FireEvent(newStateName);
    }
    //private void FireEvent(string eventName)
    //{
    //    switch (eventName)
    //    {
    //        case "Street":
    //            {
    //                if (gameEventNotUsed[0]) //Making sure each event is triggered once.
    //                {
    //                    FireOnNewGameEvent();
    //                    gameEventNotUsed[0] = false;
    //                }
    //                break;
    //            }
    //        case "Parking":
    //            {
    //                if (gameEventNotUsed[1])
    //                {
    //                    FireOnParkingEvent();
    //                    gameEventNotUsed[1] = false;
    //                }
    //                break;
    //            }
    //        case "College":
    //            {
    //                if (gameEventNotUsed[2])
    //                {
    //                    FireOnCollegeAreaEvent();
    //                    gameEventNotUsed[2] = false;
    //                }
    //                break;
    //            }
    //        case "SecondFloor":
    //            {
    //                if (gameEventNotUsed[3])
    //                {
    //                    FireOnSecondFloorEvent();
    //                    gameEventNotUsed[3] = false;
    //                }
    //                break;
    //            }
    //        case "SecretLab":
    //            {
    //                if (gameEventNotUsed[4])
    //                {
    //                    Debug.Log("Event Not Implemented Yet: " + eventName);
    //                    gameEventNotUsed[4] = false;
    //                }
    //                break;
    //            }
    //        case "SecretLabTwo":
    //            {
    //                if (gameEventNotUsed[5])
    //                {
    //                    Debug.Log("Event Not Implemented Yet: " + eventName);
    //                    gameEventNotUsed[5] = false;
    //                }
    //                break;
    //            }
    //        case "EndlessArcade":
    //            {
    //                if (gameEventNotUsed[6])
    //                {
    //                    Debug.Log("Event Not Implemented Yet: " + eventName);
    //                    gameEventNotUsed[6] = false;
    //                }
    //                break;
    //            }
    //        case "PlayerDeath":
    //            {
    //                FireOnPlayerDeathEvent();
    //                break;
    //            }
    //    }
    //}

    //protected void FireOnNewGameEvent()
    //{
    //    foreach (GameEventListener listener in listeners)
    //    {
    //        listener.OnNewGame();
    //    }
    //}
    //protected void FireOnCollegeAreaEvent()
    //{
    //    foreach (GameEventListener listener in listeners)
    //    {
    //        listener.OnCollegeArea();
    //    }
    //}
    //protected void FireOnPlayerDeathEvent()
    //{
    //    foreach (GameEventListener listener in listeners)
    //    {
    //        listener.OnPlayerDeath();
    //    }
    //}
    //protected void FireOnParkingEvent()
    //{
    //    foreach (GameEventListener listener in listeners)
    //    {
    //        listener.OnParking();
    //    }
    //}
    //protected void FireOnSecondFloorEvent()
    //{
    //    foreach (GameEventListener listener in listeners)
    //    {
    //        listener.OnSecondFloor();
    //    }
    //}
    //TODO:SecretLabFireEvents
    //protected void FireArcadeEndlessModeEvent()
    //{
    //    foreach (GameEventListener listener in listeners)
    //    {
    //        listener.OnArcadeEndlessMode();
    //    }
    //}

    //public void AddGameEventListener(GameEventListener listener)
    //{
    //    listeners.Add(listener);
    //}

    //public void RemoveGameEventListener(GameEventListener listener)
    //{
    //    listeners.Remove(listener);
    //}
}
