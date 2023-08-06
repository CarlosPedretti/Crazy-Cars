using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public Transform[] firePoints;
    public Transform[] firePointsAvaiable;

    private float inputShooting;
    private float inputMining;

    public GameObject bulletPrefab;

    public float bulletForce = 20f;
    public float fireRate = 0.1f;
    public int bulletsPerBurst = 3;
    public float burstInterval = 0.2f;

    public Transform[] minePoints;
    public GameObject minePrefab;
    public int quantityOfMines;

    public float maxHeatLevel = 400f;
    public float heatIncreasePerShot = 10f;
    public float heatDecreaseRate = 45f;

    private float nextFireTime;
    private bool isFiring;
    private bool isMining;

    public ParticleSystem overheatingParticles;

    public GameObject flashPrefab;
    public float flashPrefabDestroyTime;

    private float currentHeatLevel;

    private PlayerInput playerInput;

    [SerializeField] private FiringBar firingBar;

    public GameManager gameManager;

    private PlayerConfiguration playerConfig; // Referencia al PlayerConfiguration del jugador

    public float vibrationDuration = 10f;
    public float vibrationHighFrecuency = 0.6f;
    public float vibrationLowFrecuency = 0.4f;

    public void SetPlayerConfiguration(PlayerConfiguration config)
    {
        playerConfig = config;
    }

    public void SetInputShoot(float shoot)
    {
        inputShooting = shoot;

    }
    public void SetInputMine(float mine)
    {
        inputMining = mine;
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        nextFireTime = 0f;
        isFiring = false;
        isMining = false;
        currentHeatLevel = 0f;
        firingBar.SetMaxHeat(maxHeatLevel);
    }


    private void Update()
    {
        //float FireInput = playerInput.actions["Shoot"].ReadValue<float>();
        //float MineInput = playerInput.actions["Mines"].ReadValue<float>();

        float FireInput = inputShooting;
        float MineInput = inputMining;

        firingBar.SetHeat(currentHeatLevel);

        if (FireInput > 0 && !isFiring && !isMining && currentHeatLevel < maxHeatLevel)
        {
            isFiring = true;
            nextFireTime = Time.time;
            FireBurst();
            //StartVibration();
        }

        if (FireInput == 0)
        {
            isFiring = false;
            //StopVibration();

        }

        if (MineInput > 0 && !isMining)
        {
            isMining = true;
            Mine();
        }

        if (MineInput == 0)
        {
            isMining = false;
        }

        // Reduce el nivel de calentamiento con el tiempo
        if (!isFiring)
        {
            currentHeatLevel -= heatDecreaseRate * Time.deltaTime;
            currentHeatLevel = Mathf.Clamp(currentHeatLevel, 0f, maxHeatLevel);

        }

        float heatThreshold = maxHeatLevel * 0.8f;

        if (currentHeatLevel >= heatThreshold)
        {
            ActivateOverheatingParticles();
        }
        else
        {
            DeactivateOverheatingParticles();
        }


    }


    private void ActivateOverheatingParticles()
    {

        if (overheatingParticles != null && !overheatingParticles.isPlaying)
        {
            overheatingParticles.Play();
        }
    }

    private void DeactivateOverheatingParticles()
    {

        if (overheatingParticles != null && overheatingParticles.isPlaying)
        {
            overheatingParticles.Stop();
            //overheatingParticles.Clear();
        }
    }


    private void FireBurst()
    {
        if (currentHeatLevel + (bulletsPerBurst * heatIncreasePerShot) <= maxHeatLevel)
        {
            for (int i = 0; i < bulletsPerBurst; i++)
            {
                for (int j = 0; j < firePoints.Length; j++)
                {
                    Shoot(firePoints[j]);
                    currentHeatLevel += heatIncreasePerShot;
                }
            }

            if (isFiring)
            {
                Invoke("FireBurst", burstInterval);
            }
        }
    }




    public void Shoot(Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.SetShooter(playerConfig.PlayerIndex, gameManager); // Pasar la referencia del GameManager al Bullet
        }

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        }

        GameObject flashInstance = Instantiate(flashPrefab, firePoint.position, firePoint.rotation);
        Destroy(flashInstance, flashPrefabDestroyTime);

        Destroy(bullet, 15f);


    }

    /*public void StartVibration()
    {
        if (playerConfig != null && playerConfig.PlayerGamepad != null)
        {
            playerConfig.PlayerGamepad.SetMotorSpeeds(vibrationLowFrecuency, vibrationHighFrecuency);
            Debug.Log("Vibrando!!!!");
        }
    }

    public void StopVibration()
    {
        if (playerConfig != null && playerConfig.PlayerGamepad != null)
        {
            playerConfig.PlayerGamepad.SetMotorSpeeds(0, 0);
            Debug.Log("NO VIBRO");
        }
    }*/


    public void Mine()
    {
        if (quantityOfMines > 0)
        {
            Transform minePoint = minePoints[quantityOfMines % minePoints.Length];
            GameObject mine = Instantiate(minePrefab, minePoint.position, minePoint.rotation);

            quantityOfMines--;
            currentHeatLevel += heatIncreasePerShot;
        }
    }
}
