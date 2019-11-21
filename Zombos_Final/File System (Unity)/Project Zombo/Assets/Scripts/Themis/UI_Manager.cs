using UnityEngine;
using UnityEngine.UI;
//v2
public class UI_Manager : MonoBehaviour
{
    Text locationText;
    Slider healthSlider;
    Slider staminaSlider;

    Text MagCapacityText;
    Text BulletsInMagText;
    Text KillsText;
    Text HeadshotsText;

    Text waveText;
    private bool showWave;
    private int currentWave;
    private float waveTimer = 8.0f;
    Text zombosAliveText;

    private string currentGameState;
    private int UI_Health;
    private float UI_Stamina;

    private int MagCapacity;
    private int BulletsInMag;

    private int Kills;
    private int Headshots;

    private Stats_Manager statsManager;

    public void Start()
    {
        this.statsManager = this.GetComponent<Stats_Manager>();

        if (statsManager != null)
        {
            this.currentGameState = statsManager.GetGameState();
            this.UI_Health = statsManager.GetPlayerHealth();
            this.UI_Stamina = statsManager.GetPlayerStamina();
            this.MagCapacity = statsManager.GetMagSize();
            this.BulletsInMag = statsManager.GetBulletsInMag();
            //Debug.Log("UI BUGFIX" + currentGameState +" "+ UI_Stamina +" "+ UI_Health);
        }
        else
        {
            Debug.Log("ERROR: UI_m can't find stats manager.");
        }        
        this.locationText = GameObject.Find("LocationText").GetComponent<Text>();
        this.locationText.text = currentGameState;
        this.healthSlider = GameObject.Find("Health").GetComponent<Slider>();
        this.healthSlider.value = UI_Health;
        this.staminaSlider = GameObject.Find("Stamina").GetComponent<Slider>();
        this.staminaSlider.value = UI_Stamina;
        this.BulletsInMagText = GameObject.Find("BulletsInMagText").GetComponent<Text>();
        this.BulletsInMagText.text = BulletsInMag.ToString();
        this.MagCapacityText = GameObject.Find("MagText").GetComponent<Text>();
        this.MagCapacityText.text = MagCapacity.ToString();
        this.KillsText = GameObject.Find("KillsText").GetComponent<Text>();
        this.KillsText.text = Kills.ToString();
        this.HeadshotsText = GameObject.Find("HeadshotsText").GetComponent<Text>();
        this.HeadshotsText.text = Headshots.ToString();

        this.waveText = GameObject.Find("WaveText").GetComponent<Text>();
        this.zombosAliveText = GameObject.Find("ZombosAliveText").GetComponent<Text>();
    }
    public void UI_Update_Health(int newPlayerHealth)
    {
        if (UI_Health != newPlayerHealth)
        {
            //PLAY_UI_ANIM();
            UI_Health = newPlayerHealth;
            this.healthSlider.value = UI_Health;
        }
    }
    public void UI_Update_Stamina(float newPlayerStamina)
    {
        if (UI_Stamina != newPlayerStamina)
        {
            UI_Stamina = newPlayerStamina;
            this.staminaSlider.value = UI_Stamina;
        }
    }

    public void UI_Update_Ammo(int newMagCapacity , int newBulletsInMag)
    {
        if (MagCapacity != newMagCapacity && BulletsInMag!= newBulletsInMag)
        {
            MagCapacity = newMagCapacity;
            this.MagCapacityText.text = MagCapacity.ToString();

            BulletsInMag = newBulletsInMag;
            this.BulletsInMagText.text = BulletsInMag.ToString();
        }
    }

    public void UI_Update_BulletsInMag(int newBulletsInMag)
    {
        if (BulletsInMag!= newBulletsInMag)
        {
            if (BulletsInMag <= 10)
            {
                BulletsInMagText.color = Color.red;
            }
            else BulletsInMagText.color = Color.white;

            BulletsInMag = newBulletsInMag;
            this.BulletsInMagText.text = BulletsInMag.ToString();
            
        }
       
    }
    public void UI_Update_Wave(int wave)
    {
        showWave = true;
        currentWave = wave;
    }
    public void UI_Update_Zombos_Alive(int zombosAlive)
    {
       zombosAliveText.text = "ZOMBOS ALIVE: " + zombosAlive;
    }

    private void Update()
    {
        UpdateBulletsInMagUI();
        UpdateScoreInUI();
        UpdateKillsInUI();
        UpdateHeadshotsInUI();

        if (showWave)
        {
            waveTimer -= Time.deltaTime;
            waveText.text = "WAVE " + currentWave;

            if (waveTimer <= 3.0f)
            {
                waveText.text = "";
                waveText.CrossFadeAlpha(1f,0,true);
                waveTimer = 8.0f;
                showWave = false;                
            }
            else if(waveTimer <= 7.0f && waveTimer >= 5.5f)
            {
                waveText.CrossFadeAlpha(0f,0.9f,true);
            }
        }
    }

    public void UpdateInteractionInUI(bool shouldShow)
    {
        if (shouldShow)
        {
            Debug.Log("Press E to interact");
            //TODO: some text enable instant
        }
        else
        {
            //TODO: some text disable instant
        }
    }

    public void UpdateBulletsInMagUI()
    {
        BulletsInMagText.text = statsManager.GetBulletsInMag().ToString();
    }

    public void UpdateScoreInUI()
    {
        //Score maybe?
    }

    public void UpdateKillsInUI()
    {
        KillsText.text = statsManager.GetKills().ToString();
    }

    public void UpdateHeadshotsInUI()
    {
        HeadshotsText.text = statsManager.GetHeadshots().ToString();
    }

    public void UI_Update_Game_State(string newGameState)
    {
        if (this.currentGameState != newGameState)
        {
            this.currentGameState = newGameState;
            this.locationText.text = currentGameState;
        }
    }
}
