using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterEvent : MonoBehaviour
{
    public Transform exitCollider;
    public string changeGameStateToThis;
    public string changeObjectiveTextToThis;

    //public AudioSource voiceHelperGuidance;  Note: OnPlayer

    private readonly string playerTag = "Player";
    private GameManager gameManager;

	public void Awake ()
    {
        this.gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            gameManager.ChangeGameState(changeGameStateToThis);

            //gameManager.ChangeObjectiveDisplay(changeObjectiveTextToThis);
            //playAudioGuidanceXClip();

            exitCollider.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
