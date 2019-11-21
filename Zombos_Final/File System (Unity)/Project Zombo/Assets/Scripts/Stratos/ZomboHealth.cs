using UnityEngine;
using UnityEngine.AI;
//v2
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ZomboMovement))]
[RequireComponent(typeof(ZomboAttack))]

public class ZomboHealth : MonoBehaviour
{
    private float zomboHealth = 100;

    private bool roidRage = false; //roidRage = berserk
    private bool ableToTakeDamage = true;

    //TODO: add different types of zomboHealth/reaction

    private NavMeshAgent navAgent;
    private AI_Manager aiManager;
    private ZomboMovement zomboMov;
    private ZomboAttack zomboAtk;

    void Start()
    {
        //refs to self and daddy
        this.navAgent = this.GetComponent<NavMeshAgent>();
        this.zomboMov = this.GetComponent<ZomboMovement>();
        this.zomboAtk = this.GetComponent<ZomboAttack>();
        this.aiManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<AI_Manager>();

        if (aiManager == null)
        {
            Debug.Log("ERROR: Can't find AI manager. SOURCE: " + this.transform.name);
        }
    }

    public void ApplyDamage(float amount, int bodyPart) //TODO: add gunType (1 for AR, 2 for pistol and so on).
    {
        if (ableToTakeDamage) //This is here to make sure Die is called 1 time per zombo.
        {
            if (!roidRage) //first hit awakes the zombie
            {
                zomboHealth -= amount * bodyPart; //2x Headshot damage 1x Normal damage

                zomboMov.OnAware();
                roidRage = true;

                if (zomboHealth <= 0)
                {
                    zomboHealth = 0;
                    Die(bodyPart);
                    ableToTakeDamage = false;
                }
            }
            else
            {
                zomboHealth -= amount * bodyPart;//2x Headshot damage 1x Normal damage

                zomboMov.OnAware();

                if (zomboHealth <= 0)
                {
                    zomboHealth = 0;
                    Die(bodyPart);
                    ableToTakeDamage = false;
                }
                else if (zomboHealth <= 50) //ROID RAGE BOIS
                {
                    navAgent.acceleration = 10.0f;  //TODO: TESTS 
                    navAgent.speed = 6.0f;
                    int i = 2;
                    zomboAtk.MultiplyZomboAtkSpeed(i);
                }
            }
        }
    }

    public void SetZomboHealth(int amount) //CALLED ONLY FROM AI MANAGER
    {
        this.zomboHealth = amount;
    }
    public void AI_ManagerTerminator()
    {
        Die(2);
    }
    private void Die(int bodyPart)
    {
        var id = this.zomboMov.GetID();

        aiManager.ZomboDeath(bodyPart, id);
        navAgent.isStopped = true;
        gameObject.GetComponent<Animator>().SetBool("IsDying", true);
        Destroy(gameObject, 5f);
    }
}
