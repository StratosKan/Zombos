using UnityEngine;
using UnityEngine.AI;
//v3
[RequireComponent(typeof(ZomboAttack))]
public class ZomboMovement : MonoBehaviour
{
    private Transform target;
    private RaycastHit hit;
    private NavMeshHit navHit;
    private NavMeshAgent agent;
    private int myID;

    private float fov = 120f; //field of view
    private float viewDistance = 10f;
    private float wanderRadius = 9.0f;
    private Vector3 wanderPoint; //the wandering point our AI generates to wander arround and ACT like a zombo.

    private float zomboAttackRange = 2f; //is Here to check playerInRange

    private string playerTag = "Player";
    private bool playerInRange = false;

    private bool isAware = false;

    void Start()
    {
        //setting up refs
        if (target == null && GameObject.FindGameObjectWithTag(playerTag))
        {
            target = GameObject.FindGameObjectWithTag(playerTag).transform;  //TODO: optimization so we can get Vector3 and get the .position.
        }

        this.agent = this.GetComponent<NavMeshAgent>();
        this.wanderPoint = RandomWanderPoint();
    }

    public void Chase(Transform target)
    {
        this.agent.SetDestination(target.position);

        float distance = (target.position - this.transform.position).magnitude;

        if (distance <= zomboAttackRange)
        {
            playerInRange = true;
        }
    }

    //ABORTED ON FINAL VERSION
    public void SearchForPlayer()                                                             //Simplified version of SearchFor(Transform target).
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(target.position)) < fov / 2)  //Checks if player is within zombo fov...
        {
            float distance = (target.position - this.transform.position).magnitude;     // The distance between Zombo and Player.     

            if (distance < viewDistance)
            {
                if (Physics.Linecast(this.transform.position, target.position, out hit, -1))  //Checks if player is behind an object.
                {
                    if (hit.transform.CompareTag(playerTag))
                    {
                        OnAware();
                    }
                }
            }
        }
    }
    //ABORTED ON FINAL VERSION
    public void Wander()
    {
        if (Vector3.Distance(this.transform.position, wanderPoint) < 3.0f)
        {
            wanderPoint = RandomWanderPoint();
        }
        else
        {
            agent.SetDestination(wanderPoint);
        }
    }
    //ABORTED ON FINAL VERSION
    public Vector3 RandomWanderPoint() //TODO: optimization.
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position; //Creates a random point in wanderRadius

        randomPoint.y = this.transform.position.y; //EDITED ON: 06/02/19 V3

        NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, -1);                  //...then returns a hit on nav mesh (Careful with nav mesh on other floors)

        return new Vector3(navHit.position.x, this.transform.position.y, navHit.position.z); //... and finally sends back the wanderPoint vector.
    }

    /*
     * The following methods are called from AI Manager to check AI status.
     */

    public void OnAware()          //OnAware used to improve Agent Speed as well.
    {
        isAware = true;
    }
    public bool AwareOrNot()
    {
        return isAware;
    }
    public bool IsPlayerInRange()
    {
        return playerInRange;
    }
    public void ResetPlayerInRange()
    {
        playerInRange = false;
    }
    public int GetID()
    {
        return myID;
    }
    public void ChangeMyID(int id)
    {
        myID = id;
    }

    //MOVED TO AI_MANAGER
    //void Update ()
    //   {
    //       if (isAware) //ai(i).isAware
    //       {
    //           Chase(target);
    //           zomboAttackTimer -= Time.deltaTime; //todo: gameplay test if it should be inside playerInRange bool.

    //           if (playerInRange)
    //           {
    //               if (zomboAttackTimer <= 0) //in this version attack speed is on playerMovement because update runs on zomboMovement.
    //               {
    //                   zomboAtk.Attack(target);
    //                   zomboAttackTimer = zomboAttackSpeed;
    //               }
    //               playerInRange = false;
    //           }

    //           //TODO: Evasive maneuvers            
    //           zomboRenderer.material.color = Color.yellow;
    //       }
    //       else
    //       {
    //           //this.agent.SetDestination(dummyTargetTest.position);
    //           SearchForPlayer();
    //           Wander();
    //           zomboRenderer.material.color = Color.blue;
    //       }
    //}

    //void OnNavMeshPreUpdate() // use for callbacks to encounter before nav mesh calcs.
}
