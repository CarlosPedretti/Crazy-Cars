using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public GameObject deathPrefab;
    public float deathPrefabDestroyTime;
    public float playerDestroyDelay;

    [SerializeField] private HealthBar healthBar;


    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Vehiculo destruido");
            Die();
        }
    }

    public void Die()
    {

        if (deathPrefab != null)
        {
            GameObject deathInstance = Instantiate(deathPrefab, transform.position, transform.rotation);
            Destroy(deathInstance, deathPrefabDestroyTime);
        }
        Invoke("DestroyPlayer", playerDestroyDelay);


    }

    private void DestroyPlayer()
    {
       Destroy(gameObject);
    }



}
