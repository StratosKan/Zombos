using UnityEngine;
//v1
//BUILT WITH RESPONSIVENESS AS MAIN GOAL
//WILL BE OPTIMISED IN LATER VERSION
[RequireComponent(typeof(PlayerMovementScript))]
public class PlayerHealth : MonoBehaviour
{
    private int playerHealth; //player health is integer for UI and clarity purposes.
    private int MAX_ALLOWED_PLAYER_HEALTH;

    private float playerStamina; //stamina has to be float for regeneration.
    private float staminaRegen = 5f; //TODO TESTS!
    private float MAX_ALLOWED_PLAYER_STAMINA;

    private float timeSinceLastStaminaDrain;
    private float defaultTimeSinceLastStaminaDrain = 3f; //3 seconds to start stamina regen.

    private int playerArmor;

    private int stimpacksOnMe;
    private int stimpacksUsed;
    private int stimpackHealAmount = 30;

    private bool shouldHeal = false;
    private bool shouldRun = false;

    private Stats_Manager stats_manager;
    private InputManager input_manager;
    private PlayerMovementScript player_movement;

	void Start ()
    {
        this.stats_manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Stats_Manager>();
        this.input_manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>();
        this.player_movement = this.GetComponent<PlayerMovementScript>();

        if(stats_manager == null)
        {
            Debug.Log("ERROR: Can't find stats manager! Source: Player");
        }
        else
        {
            this.playerHealth = stats_manager.GetPlayerHealth();

            this.playerStamina = stats_manager.GetPlayerStamina();

            this.playerArmor = stats_manager.GetPlayerArmor();

            this.MAX_ALLOWED_PLAYER_HEALTH = stats_manager.GetMaxPlayerHealth(); //We are going to save playerHealth so it must be separate.

            this.MAX_ALLOWED_PLAYER_STAMINA = stats_manager.GetMaxPlayerStamina();

            this.timeSinceLastStaminaDrain = this.defaultTimeSinceLastStaminaDrain;

            this.stimpacksOnMe = stats_manager.GetStimpacksOnPlayer();

            //Debug.Log("PLAYER HEALTH: " + playerHealth + " PLAYER STAMINA: " + playerStamina + " PLAYER ARMOR: " + playerArmor);

            if (playerArmor == 0 || playerStamina == 0 || playerHealth == 0)
            {
                Debug.Log("ERROR: Can't get player stats on player.");
            }
        }
    }

    private void Update()
    {
        CheckRunInput();
        CheckHealInput();
    }

    public void CheckHealInput()
    {
        shouldHeal = false;
        if (input_manager.StimpackUse)
        {
            shouldHeal = !shouldHeal;
        }
        if (shouldHeal)
        {
            if(stimpacksOnMe > 0)
            {
                if (playerHealth < MAX_ALLOWED_PLAYER_HEALTH)
                {
                    HealPlayer(stimpackHealAmount, 1);
                }
                else
                {
                    Debug.Log("ALREADY AT MAX HEALTH");
                    //TODO UI MESSAGE
                }
            }
        }
    }

    public void CheckRunInput()
    {
        timeSinceLastStaminaDrain -= Time.deltaTime;

        shouldRun = false;
        player_movement.playerShouldRun = false;

        if (input_manager.IsRunning)
        {
            shouldRun = !shouldRun;
        }

        if (shouldRun)
        {
            StaminaDrain();

            if (playerStamina > 0)
            {
                player_movement.playerShouldRun = true;
            }
        }

        if (timeSinceLastStaminaDrain <= 0)
        {
            StaminaRegen();
        }
    }

    public void TakeDamage(int amount)
    {
        this.playerHealth -= (amount - playerArmor); //first we apply armor mitigation then we extract from playerHealth.

        stats_manager.SetPlayerHealth(this.playerHealth);

        if (this.playerHealth <= 0)
        {
            playerHealth = 0;
            Die();
        }
    }
    public void HealPlayer(int amount,int type) //1 for stimpack, 2 for everything else
    {
        if (type == 1)
        {
            this.playerHealth += amount;

            if (this.playerHealth >= MAX_ALLOWED_PLAYER_HEALTH)
            {
                this.playerHealth = MAX_ALLOWED_PLAYER_HEALTH;
            }
            stimpacksOnMe--;
            stimpacksUsed++;
            stats_manager.OneMoreOnTheHouse();
            stats_manager.SetPlayerHealth(this.playerHealth);
        }
        else
        {
            this.playerHealth += amount;
            stats_manager.SetPlayerHealth(this.playerHealth);
        }
    }
    public void StaminaRegen()
    {
        this.playerStamina += (staminaRegen/2) * Time.deltaTime; //TODO tests

        if (playerStamina >= MAX_ALLOWED_PLAYER_STAMINA)
        {
            this.playerStamina = this.MAX_ALLOWED_PLAYER_STAMINA;
        }

        stats_manager.SetPlayerStamina(playerStamina);
    }

    public void StaminaDrain() //CALLED EACH TIME SHIFT IS PRESSED xTIME.
    {
        this.playerStamina -= staminaRegen * Time.deltaTime; 

        this.timeSinceLastStaminaDrain = this.defaultTimeSinceLastStaminaDrain;

        if (this.playerStamina <= 0)
        {
            playerStamina = 0;
        }

        stats_manager.SetPlayerStamina(playerStamina);
    }
    public void Die()
    {
        Debug.Log("IM DEAD");
        //stats playerDeaths++;
        //stats respawnMe;
        //UI_PLAY_DEATH_ANIM();
        //AUDIO_WHATEVER();
        
        //SCENE LOAD OCCURS FROM STATS MANAGER OR GAME MANAGER NOT FROM PLAYER. TY
    }
    public void AddStimPack()
    {
        stimpacksOnMe++;
        Debug.Log("PLAYERHEALTH: Stimpack added: " + stimpacksOnMe);
        //UPDATE UI through Stats
    }
}
