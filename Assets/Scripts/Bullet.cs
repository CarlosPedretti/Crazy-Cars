using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public GameObject hitPrefab;
    public float hitPrefabDestroyTime;

    private int shooterIndex;
    private GameManager gameManager; 

    public void SetShooter(int playerIndex, GameManager gm)
    {
        shooterIndex = playerIndex;
        gameManager = gm;
        Debug.Log("Shooter Index: " + shooterIndex); 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health health = other.gameObject.GetComponent<Health>();

            if (health != null)
            {
               
                health.TakeDamage(damage);

               
                if (health.GetCurrentHealth() <= 0)
                {
                  
                    if (shooterIndex >= 0)
                    {
                       
                        if (gameManager != null)
                        {
                            gameManager.IncreaseScore(shooterIndex);
                            Debug.Log("Score increased for Player " + shooterIndex); 
                        }
                    }
                }
            }

            Quaternion hitRotation = Quaternion.LookRotation(-transform.forward);
            GameObject hitInstance = Instantiate(hitPrefab, transform.position, hitRotation);
            Destroy(hitInstance, hitPrefabDestroyTime);
        }

        Destroy(gameObject);
    }
}
