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
    public float dieDelay;

    [SerializeField] private HealthBar healthBar;

    public ParticleSystem smokeParticles;
    public ParticleSystem fireParticles;


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
            Invoke("Die", dieDelay);
        }
    }

    public void TakeHeal(int healAmount)
    {
        currentHealth += healAmount;

        healthBar.SetHealth(currentHealth);

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;

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


    void Update()
    {
        float smokeThreshold = maxHealth * 0.35f;
        float fireThreshold = maxHealth * 0.1f;

        if (currentHealth <= smokeThreshold)
        {
            ActivateSmokeParticles();
        }
        else
        {
            DesactivateSmokeParticles();
        }

        if (currentHealth <= fireThreshold)
        {
            ActivateFireParticles();
        }
        else
        {
            DesactivateFireParticles();
        }
    }

    private void ActivateFireParticles()
    {

        if (fireParticles != null && !fireParticles.isPlaying)
        {
            fireParticles.Play();
        }
    }

    private void DesactivateFireParticles()
    {

        if (fireParticles != null && fireParticles.isPlaying)
        {
            fireParticles.Stop();
            //fireParticles.Clear();
        }
    }

    private void ActivateSmokeParticles()
    {

      if (smokeParticles != null && !smokeParticles.isPlaying)
      {
        smokeParticles.Play();
      }
    }

    private void DesactivateSmokeParticles()
    {

      if (smokeParticles != null && smokeParticles.isPlaying)
      {
        smokeParticles.Stop();
        //smokeParticles.Clear();
         
      }
    }



}
