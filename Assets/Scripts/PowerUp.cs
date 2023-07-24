using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUp : MonoBehaviour
{
    public PowerUpData powerUpData;
    public float powerUpDuration = 10f;
    private Coroutine revertChanges;

    private bool valuesSaved = false;

    private float originalBulletForce;
    private float originalFireRate;
    private int originalBulletsPerBurst;
    private int originalQuantityOfMines;
    private float originalMaxHeatLevel;
    private float originalHeatIncreasePerShot;
    private float originalHeatDecreaseRate;
    private Transform[] originalFirePoints;
    private Transform[] originalFirePointAvaiable;

    private PowerUpSpawner spawner;

    public void SetSpawner(PowerUpSpawner powerUpSpawner)
    {
        spawner = powerUpSpawner;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Weapon weapon = other.GetComponent<Weapon>();

            if (weapon != null)
            {
                if (!valuesSaved) 
                {
                    SaveOriginalValues(weapon);
                    valuesSaved = true; 
                }

                if (revertChanges == null) // Comprueba si revertChanges no está en ejecución
                {
                    // Aplicar los cambios del PowerUp al componente Weapon
                    // Resto del código existente...

                    revertChanges = StartCoroutine(RevertChanges(weapon, originalQuantityOfMines, originalMaxHeatLevel, originalHeatIncreasePerShot, originalHeatDecreaseRate, originalBulletForce, originalFireRate, originalBulletsPerBurst, originalFirePoints));
                }

                //Aplicar los cambios del PowerUp al componente Weapon
                weapon.bulletForce = powerUpData.bulletForce;
                weapon.fireRate = powerUpData.fireRate;
                weapon.bulletsPerBurst = powerUpData.bulletsPerBurst;
                weapon.quantityOfMines = powerUpData.quantityOfMines;
                weapon.firePoints = GetFirePointsFromIndices(originalFirePointAvaiable, powerUpData.firePointIndices);
                weapon.maxHeatLevel = powerUpData.maxHeatLevel;
                weapon.heatIncreasePerShot = powerUpData.heatIncreasePerShot;
                weapon.heatDecreaseRate = powerUpData.heatDecreaseRate;



                MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer childRenderer in meshRenderers)
                {
                    childRenderer.enabled = false;
                }
            }

            if (spawner != null)
            {
                spawner.PowerUpCollected(gameObject);
            }

        }
    }


    private void SaveOriginalValues(Weapon weapon)
    {
        // Guardar los valores originales del componente Weapon
        originalBulletForce = weapon.bulletForce;
        originalFireRate = weapon.fireRate;
        originalBulletsPerBurst = weapon.bulletsPerBurst;
        originalQuantityOfMines = weapon.quantityOfMines;
        originalMaxHeatLevel = weapon.maxHeatLevel;
        originalHeatIncreasePerShot = weapon.heatIncreasePerShot;
        originalHeatDecreaseRate = weapon.heatDecreaseRate;
        originalFirePoints = weapon.firePoints;
        originalFirePointAvaiable = weapon.firePointsAvaiable;
    }

    private IEnumerator RevertChanges(Weapon weapon, int originalQuantityOfMines, float originalMaxHeatLevel, float originalHeatIncreasePerShot, float originalHeatDecreaseRate, float originalBulletForce, float originalFireRate, int originalBulletsPerBurst, Transform[] originalFirePoints)
    {
        float elapsedTime = 0f;

        while (elapsedTime < powerUpDuration)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Asegúrate de que los cambios solo se reviertan si efectivamente se había activado el Coroutine
        if (revertChanges != null)
        {
            weapon.bulletForce = originalBulletForce;
            weapon.fireRate = originalFireRate;
            weapon.bulletsPerBurst = originalBulletsPerBurst;
            weapon.firePoints = originalFirePoints;
            weapon.quantityOfMines = originalQuantityOfMines;
            weapon.maxHeatLevel = originalMaxHeatLevel;
            weapon.heatIncreasePerShot = originalHeatIncreasePerShot;
            weapon.heatDecreaseRate = originalHeatDecreaseRate;

            Destroy(gameObject);
        }
    }

    private Transform[] GetFirePointsFromIndices(Transform[] originalFirePointAvaiable, List<int> firePointIndices)
    {
        Transform[] firePoints = new Transform[firePointIndices.Count];

        for (int i = 0; i < firePointIndices.Count; i++)
        {
            int index = firePointIndices[i];
            if (index >= 0 && index < originalFirePointAvaiable.Length)
            {
                firePoints[i] = originalFirePointAvaiable[index];
            }
        }


        return firePoints;
    }
}
