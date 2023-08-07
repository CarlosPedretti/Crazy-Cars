using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
    public List<GameObject> powerUps; 
    public float minSpawnCooldown = 3f; 
    public float maxSpawnCooldown = 6f; 
    public int maxPowerUps = 5; 

    private List<Transform> spawnPoints; 
    private List<GameObject> spawnedPowerUps; 

    private float currentCooldown;

    private void Start()
    {
        spawnPoints = new List<Transform>(GetComponentsInChildren<Transform>());
        spawnedPowerUps = new List<GameObject>();


        spawnPoints.Remove(transform);


        currentCooldown = Random.Range(minSpawnCooldown, maxSpawnCooldown);
    }

    private void Update()
    {
        if (currentCooldown <= 0 && spawnedPowerUps.Count < maxPowerUps)
        {
            SpawnPowerUp();
            currentCooldown = Random.Range(minSpawnCooldown, maxSpawnCooldown);
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    private void SpawnPowerUp()
    {
        if (powerUps.Count == 0)
        {
            //Debug.LogWarning("La lista de PowerUps está vacía.");
            return;
        }

        if (spawnPoints.Count == 0)
        {
            //Debug.LogWarning("No hay puntos de spawn disponibles.");
            return;
        }

        if (spawnedPowerUps.Count >= maxPowerUps)
        {
            //Debug.LogWarning("Se ha alcanzado la cantidad máxima de PowerUps en la escena.");
            return;
        }


        int randomSpawnIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomSpawnIndex];


        int randomPowerUpIndex = Random.Range(0, powerUps.Count);
        GameObject powerUpPrefab = powerUps[randomPowerUpIndex];


        GameObject spawnedPowerUp = Instantiate(powerUpPrefab, spawnPoint.position, Quaternion.identity);
        spawnedPowerUp.transform.SetParent(spawnPoint); 
        spawnedPowerUps.Add(spawnedPowerUp);


        PowerUp powerUp = spawnedPowerUp.GetComponent<PowerUp>();
        if (powerUp != null)
        {
            powerUp.SetSpawner(this);
        }


        spawnPoints.RemoveAt(randomSpawnIndex);
    }

    public void PowerUpCollected(GameObject powerUp)
    {
        if (spawnedPowerUps.Contains(powerUp))
        {
            spawnedPowerUps.Remove(powerUp);
            //Destroy(powerUp);


            currentCooldown = Random.Range(minSpawnCooldown, maxSpawnCooldown);


            spawnPoints.Add(powerUp.transform.parent);
        }
    }
}
