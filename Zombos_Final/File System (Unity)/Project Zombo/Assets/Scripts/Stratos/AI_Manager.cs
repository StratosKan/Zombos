using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

//v4(final)
[RequireComponent(typeof(Stats_Manager))]

public class AI_Manager : MonoBehaviour
{
    private int currentWave = 0; //TODODOODODO
    private bool currentDifficulty = true; //Default set to hard.
    private int MAX_ALLOWED_ZOMBOS = 40;

    public GameObject zomboPrefab;
    public Transform dadOFZombos;

    private GameObject[] activeAgents = new GameObject[350];                  //reference to everything AI related so we can use for our needs.
    private Vector3[] activeAgentsPositions = new Vector3[350];

    private ZomboAttack[] activeAgentsAtkScripts = new ZomboAttack[350];
    private ZomboMovement[] activeAgentsMovementScripts = new ZomboMovement[350];
    private ZomboHealth[] activeAgentsHealthScripts = new ZomboHealth[350];
    private NavMeshAgent[] activeAgentsNavMesh = new NavMeshAgent[350];
    private Animator[] activeAgentsAnimator = new Animator[350];
    private AudioSource[] activeAgentsAudiosource = new AudioSource[350];
    private float[] activeAgentsAttackTimer = new float[350];

    private int[] activeAgentsID = new int[350];
    private bool[] activeAgentsEnabled = new bool[350];
    
    private int zombosSpawned = 0; //This serves as a counter and an ID for zombos.
    private int zombosAlive = 0;
    private int zombosKilledInSession = 0;
    private int zombosKilledWithHeadshot = 0; //deprecated because of model

    private Transform spawnPointsParkingDad;
    private int spawnChildrenCount;
    public Vector3[] spawnPoints; //spawnPoint Control.    
    private int randomSpawnPoint;

    [Header("Experimental Option")]
    public bool endlessSpawn;
    public int howManyToSpawnOnStart;
    public float zomboRespawnTimer;  //TODO: MAKE THIS AN OPTION AND/OR MAKE THIS WORK IN 
    private float defaultZomboRespawnTimer;
    private bool wavePlaying = false;

    private Stats_Manager stats_manager;
    private GameManager gameManager;

    private string playerTag = "Player";
    private Transform target;
    private float defaultZomboAttackTimer = 4f;

    public float rotationalSpeed = 3f; //For facing the player

    void Start()
    {
        //TODO: Get stats_manager stats.

        //TODO: GET GAME STATE!

        this.gameManager = this.GetComponent<GameManager>();
        this.stats_manager = this.GetComponent<Stats_Manager>();

        if (target == null && GameObject.FindGameObjectWithTag(playerTag))
        {
            target = GameObject.FindGameObjectWithTag(playerTag).transform;  //TODO: optimization so we can get Vector3 and get the .position.
        }

        //TODO: NEST THIS WITH GAME STATE
        //IF STATE PARKING
        spawnPointsParkingDad = GameObject.Find("ZomboSpawnPointsParking").transform; //Finding papa

        CountTheChildren(spawnPointsParkingDad);
        //END IF

        if (howManyToSpawnOnStart > 0)
        {
            MassiveZomboRandomSpawn(howManyToSpawnOnStart);
        }

        defaultZomboRespawnTimer = zomboRespawnTimer;
    }

    void FixedUpdate()
    {
        if (zombosAlive > 0)
        {
            foreach (int ai in activeAgentsID)  //Running all agent updates through this manager.
            {
                if (activeAgentsEnabled[ai]) //Checks if it should do the updates on this AI.
                {
                    //Anim
                    //activeAgentsMovementScripts[ai].gameObject.GetComponent<Animator>().SetBool("IsWalking", true);

                    activeAgentsAnimator[ai].SetFloat("WalkSpeed", activeAgentsNavMesh[ai].speed);
                    //Anim

                    if (activeAgentsMovementScripts[ai].AwareOrNot()) //final version: ALL ZOMBOS AWARE MAYBE REMOVE
                    {
                        activeAgentsMovementScripts[ai].Chase(target);
                        //Debug.Log(activeAgentsID[ai] + " Chasing player");
                        TerminateSingleAgent(0); //Lil bugger
                        activeAgentsAttackTimer[ai] -= Time.deltaTime;

                        if (activeAgentsMovementScripts[ai].IsPlayerInRange())
                        {
                            if (activeAgentsAttackTimer[ai] <= 0)
                            {
                                //Anim
                                //Rotate towards player
                                Vector3 lookPos = target.position - activeAgents[ai].transform.position;
                                lookPos.y = 0;
                                Quaternion rotation = Quaternion.LookRotation(lookPos);
                                activeAgents[ai].transform.rotation = Quaternion.Slerp(activeAgents[ai].transform.rotation, rotation, rotationalSpeed);
                                //Rotate towards player

                                activeAgentsAnimator[ai].SetBool("IsWalking", false);

                                activeAgentsAnimator[ai].SetBool("IsAttacking", true);
                                //Anim
                                //Audio
                                activeAgentsAudiosource[ai].Play();
                                //Audio
                                activeAgentsAtkScripts[ai].Attack(target);

                                activeAgentsAttackTimer[ai] = defaultZomboAttackTimer;
                            }

                            activeAgentsMovementScripts[ai].ResetPlayerInRange();
                        }
                        else
                        {
                            //Debug.Log("We are here!");
                            //Anim
                            activeAgentsAnimator[ai].SetBool("IsAttacking", false);

                            activeAgentsAnimator[ai].SetBool("IsWalking", true);
                            //Anim
                        }
                    }
                    else
                    {
                        //DEPRECATED BECAUSE WE SET ZOMBO AWARE BY DEFAULT
                        activeAgentsMovementScripts[ai].SearchForPlayer();
                        activeAgentsMovementScripts[ai].Wander();
                    }
                }
            }
        }
        if (endlessSpawn)
        {
            //TODO: NEST THIS
            zomboRespawnTimer -= Time.deltaTime;

            if (zomboRespawnTimer <= 0)
            {
                RandomZomboSpawn();
                zomboRespawnTimer = defaultZomboRespawnTimer;
            }
        }
        if (wavePlaying)
        {
            if (zombosAlive <= 0)
            {
                WaveFinished();
                wavePlaying = false;
            }
        }
    }
    public void SpawnNextWave(bool difficulty) //ONLY CALLED FROM GAME_MANAGER
    {
        wavePlaying = false;

        currentWave++;
        TerminateActiveAgents();

        int spawnFormula = 6 + currentWave; // max 120 + 210 = 330
        MassiveZomboRandomSpawn(spawnFormula);

        wavePlaying = true;
        StatsUpdate();
        //Debug.Log("SPAWNING WAVE " + currentWave);
    }
    public void WaveFinished()
    {
        gameManager.NextWave();
        //UPDATE UI_MANAGER
        //GameManager wave finished awaiting orders
    }

    public void CountTheChildren(Transform spawnPointsDad) //v3 made it working arround whole project. Each level will have a spawnPointsDad and the actual spawnPoints will be his children.
    {
        spawnChildrenCount = spawnPointsDad.childCount;                        //Count the children

        spawnPoints = new Vector3[spawnPointsDad.childCount];

        for (int i = 0; i < spawnChildrenCount; i++)                                //Add each children's position to the V3[].  
        {
            spawnPoints[i] = new Vector3(
                spawnPointsDad.GetChild(i).position.x,
                spawnPointsDad.GetChild(i).position.y,
                spawnPointsDad.GetChild(i).position.z
                );
        }

        //Debug.Log("Found " + spawnChildrenCount + " possible spawn points");
    }

    public void MassiveZomboRandomSpawn(int howManyZombosToSpawn)
    {
        if (howManyZombosToSpawn > 0)
        {
            for (int i = 0; i <= howManyZombosToSpawn; i++)
            {
                RandomZomboSpawn();
            }
        }
    }

    public void RandomZomboSpawn()
    {
        randomSpawnPoint = Random.Range(0, spawnChildrenCount - 1); //its inclusive MIN/MAX
        AwareZomboSpawn(spawnPoints[randomSpawnPoint]);
        //DEPRECATED: spawnPointCooldown;
    }

    public void ZomboSpawnByIndex(int index)  //Use this for trigger events.
    {
        AwareZomboSpawn(spawnPoints[index]);
    }

    public void AwareZomboSpawn(Vector3 spawnPoint)
    {
        if (zombosAlive < MAX_ALLOWED_ZOMBOS)
        {
            activeAgents[zombosSpawned] = Instantiate(zomboPrefab, spawnPoint, Quaternion.identity, dadOFZombos);

            AddZomboAsEdge(activeAgents[zombosSpawned]);

            activeAgentsMovementScripts[zombosSpawned].OnAware(); //this!!
            //TODO: IF DIFFICULTY = HARD MODIFY STATS DEPENDING ON WAVE modifyStats(int wave)

            zombosSpawned++;
            zombosAlive++;
        }
        else
        {
            Debug.Log("AI MANAGER: MAX ALLOWED ZOMBOS ALIVE REACHED.");
        }
    }
    public void AddZomboAsEdge(GameObject zombo)
    {
        //applying references for optimal behaviour/control.
        activeAgentsPositions[zombosSpawned] = zombo.transform.position;
        activeAgentsAtkScripts[zombosSpawned] = zombo.GetComponent<ZomboAttack>();
        activeAgentsHealthScripts[zombosSpawned] = zombo.GetComponent<ZomboHealth>();
        activeAgentsMovementScripts[zombosSpawned] = zombo.GetComponent<ZomboMovement>();
        activeAgentsNavMesh[zombosSpawned] = zombo.GetComponent<NavMeshAgent>();
        activeAgentsAnimator[zombosSpawned] = zombo.GetComponent<Animator>();
        activeAgentsAudiosource[zombosSpawned] = zombo.GetComponent<AudioSource>();
        activeAgentsAttackTimer[zombosSpawned] = defaultZomboAttackTimer;

        activeAgentsID[zombosSpawned] = zombosSpawned; //Assigning ID
        activeAgentsEnabled[zombosSpawned] = true; //Toggled off from ZomboDeath().        
        activeAgentsMovementScripts[zombosSpawned].ChangeMyID(zombosSpawned); //ID is on zombo as well

        if (currentDifficulty) //MEANS WE ARE ON HARD DIFFICULTY
        {
            int atkDifficultyFormula = 4 + currentWave;
            activeAgentsAtkScripts[zombosSpawned].SetZomboAtkDmg(atkDifficultyFormula);
            //Debug.Log(activeAgentsAtkScripts[zombosSpawned].GetZomboAtkDmg());

            int healthDifficultyFormula = 95 + currentWave * 5;
            activeAgentsHealthScripts[zombosSpawned].SetZomboHealth(healthDifficultyFormula);
        }
    }

    public void TerminateActiveAgents() 
    {
        foreach(int ai in activeAgentsID)
        {
            if (activeAgentsEnabled[ai])
            {
                activeAgentsHealthScripts[ai].AI_ManagerTerminator(); //bye komrade, u failed xD 
            }
        }
    }
    public void TerminateSingleAgent(int id)
    {
        if (activeAgentsEnabled[id])
        {
            activeAgentsHealthScripts[id].AI_ManagerTerminator();
        }
    }
    public void SetDifficulty(bool diff)
    {
        this.currentDifficulty = diff;
    }

    public void ZomboDeath(int bodyPart, int zomboID) //1 for anything , 2 for headshot
    {
        if (bodyPart == 2)
        {
            zombosAlive--;
            zombosKilledInSession++;
            zombosKilledWithHeadshot++;
        }
        else if (bodyPart == 1)
        {
            zombosAlive--;
            zombosKilledInSession++;
        }
        else
        {
            Debug.Log("ERROR: ZOMBO DEATH TYPE FAILURE");
        }

        activeAgentsEnabled[zomboID] = false; //Turns down the updates on that zombo.

        StatsUpdate();  //Stat tracking - Event-driven logic
    }
    //DEPRECATED
    public void ZomboSpawn(Vector3 spawnPoint) //maybe use Transform here but no need for now.
    {
        if (zombosAlive < MAX_ALLOWED_ZOMBOS)
        {
            activeAgents[zombosSpawned] = Instantiate(zomboPrefab, spawnPoint, Quaternion.identity, dadOFZombos);

            AddZomboAsEdge(activeAgents[zombosSpawned]);

            zombosSpawned++;
            zombosAlive++;
        }
        else
        {
            Debug.Log("AI MANAGER: MAX ALLOWED ZOMBOS ALIVE REACHED.");
        }
    }

    public void StatsUpdate()
    {
        stats_manager.AI_Update(this.zombosAlive, this.zombosKilledInSession, this.zombosKilledWithHeadshot);
    }
}
