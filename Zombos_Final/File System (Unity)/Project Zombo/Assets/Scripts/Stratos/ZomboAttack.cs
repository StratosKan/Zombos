using UnityEngine;
//v2
public class ZomboAttack : MonoBehaviour
{
    private float zomboAttackSpeed = 0.9f; //default was 0.7f
    private int zomboAttackDamage = 4;
    private int attackCount = 0;
    private string playerTag = "Player";
    private string enemyTag = "Enemy";

    //Called by Zombo Movement in this version. Can be called from Zombo AI manager in the future
    public void Attack(Transform target) //transform can be optimised
    {
        if (target.gameObject.CompareTag(playerTag))
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(zomboAttackDamage);
                attackCount++;
                //playAnim();  (1.06f)
                //playAudio();
            }
            else
            {
                Debug.Log("ERROR: Can't find target health " + this.transform.name);
            }
        }
        else if (target.gameObject.CompareTag(enemyTag)) // ZvZ action BOIS :)
        {
            ZomboHealth zomboHealth = target.GetComponent<ZomboHealth>();
            zomboHealth.ApplyDamage((float)zomboAttackDamage, 1);
        }
        else
        {
            Debug.Log("ERROR: Can't find target health " + this.transform.name);
        }
    }
    //GET & SET METHODS
    public int GetAtkCount()
    {
        return attackCount;
    }
    public int GetZomboAtkDmg()
    {
        return zomboAttackDamage;
    }
    public float GetZomboAtkSpeed()
    {
        return zomboAttackSpeed;
    }
    public void SetZomboAtkDmg(int amount)
    {
        this.zomboAttackDamage = amount;
    }
    public void MultiplyZomboAtkDmg(int multiplier)
    {
        this.zomboAttackDamage *= multiplier;
    }
    public void SetZomboAtkSpeed(float amount)
    {
        this.zomboAttackSpeed = amount;
    }
    public void MultiplyZomboAtkSpeed(int multiplier)
    {
        this.zomboAttackSpeed *= multiplier;
    }
}
