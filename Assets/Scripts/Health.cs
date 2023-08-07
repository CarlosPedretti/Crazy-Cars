using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public GameObject deathPrefab;
    public float deathPrefabDestroyTime;
    public float playerDestroyDelay;
    public float dieDelay;

    public float respawnTime = 5f; // Tiempo en segundos para reaparecer
    private bool isRespawning = false;

    [SerializeField] private HealthBar healthBar;

    public ParticleSystem smokeParticles;
    public ParticleSystem fireParticles;

    //public float vibrationDuration = 3.0f;
    //public float vibrationAmplitude = 1.0f;


    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            //Invoke("Die", dieDelay);
            PlayerDied();

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

    public void PlayerDied()
    {
        if (!isRespawning)
        {
            //Gamepad.current?.SetMotorSpeeds(vibrationAmplitude, vibrationAmplitude);
            //Invoke("StopVibration", vibrationDuration);

            if (deathPrefab != null)
            {
                GameObject deathInstance = Instantiate(deathPrefab, transform.position, transform.rotation);
                Destroy(deathInstance, deathPrefabDestroyTime);
            }

            // Desactivar el objeto del jugador y comenzar el proceso de reaparición
            gameObject.SetActive(false);
            Invoke("RespawnPlayer", respawnTime);
        }
    }

    /*private void StopVibration()
    {
        // Detener la vibración del mando
        Gamepad.current?.SetMotorSpeeds(0, 0);
    }*/

    private void RespawnPlayer()
    {
        // Buscar el componente RespawnPoint en el objeto
        RespawnPoint respawnPoint = GetComponent<RespawnPoint>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        if (respawnPoint != null)
        {
            // Usar la respawnPosition del componente RespawnPoint como punto de respawn
            transform.position = respawnPoint.respawnPosition;
            transform.rotation = respawnPoint.respawnRotation;
        }
        else
        {
            // Si no se encuentra el componente RespawnPoint, simplemente reiniciar la posición del jugador
            transform.position = Vector3.zero;
        }

        gameObject.SetActive(true);
        isRespawning = false;
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
