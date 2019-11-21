using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomboSpawnTriggerEvent : MonoBehaviour
{
    public bool zombosAware;
    public int howManyZombos;
    public Transform[] spawnZomboLocations;

    private AI_Manager aiManager;
    private readonly string playerTag = "Player";

    public void Awake ()
    {
		this.aiManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<AI_Manager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
                if (howManyZombos == spawnZomboLocations.Length) //TODO: If not equal
                {
                    for (int i = 0; i <= spawnZomboLocations.Length - 1; i++)
                    {
                        if (zombosAware)
                        {
                            aiManager.AwareZomboSpawn(spawnZomboLocations[i].position);
                        }
                        else
                        {
                            aiManager.ZomboSpawn(spawnZomboLocations[i].position);
                        }
                    }
                    Debug.Log("Spawned " + howManyZombos + " zombos from trigger " + this.transform.name);
                }
                this.gameObject.SetActive(false);
        }        
    }
}
