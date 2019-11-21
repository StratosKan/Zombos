using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//v1
public class GameStateMachine : MonoBehaviour
{
    private IState currentlyActiveState;
    private IState previouslyActiveState;

    public void ChangeState(IState newState)
    {
        if(this.currentlyActiveState != null) //Making sure there is an active state.
        {
            this.previouslyActiveState = this.currentlyActiveState; //Sets currentlyActiveState to previous...
            this.currentlyActiveState.Exit();                       //...before exiting.
        }
        this.currentlyActiveState = newState;    //Then, applying the new state...                 
        this.currentlyActiveState.Enter();       //...and enter it.
    }
    public void ExecuteStateUpdate()
    {
        IState runningState = this.currentlyActiveState; 

        if (runningState != null)
        {
            runningState.Execute();
        }
    }
    /* TO-DECIDE:
     * THIS SWITCH CAN BE USED ON RESPAWN SYSTEM. ENTERING LEVEL -> PLAYING LEVEL -> EXITING LEVEL
     * SO IF PLAYING LEVEL IS THE CURRENT STATE WHEN PLAYER DIES RESPAWN OCCLUDES ON ENTERING LEVEL  
     */
    public void SwitchToPreviousState()  
    {
        this.currentlyActiveState.Exit();
        this.currentlyActiveState = this.previouslyActiveState;
        this.currentlyActiveState.Enter();
    }
}
