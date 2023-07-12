using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUp : MonoBehaviour
{
    public PowerUpData powerUpData;
    public float powerUpDuration = 10f;
    private Coroutine revertChanges;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Weapon weapon = other.GetComponent<Weapon>();
            if (weapon != null)
            {
                //Guardar los valores originales del componente Weapon
                float originalBulletForce = weapon.bulletForce;
                float originalFireRate = weapon.fireRate;
                int originalBulletsPerBurst = weapon.bulletsPerBurst;
                Transform[] originalFirePoints = weapon.firePoints;
                Transform[] originalFirePointAvaiable = weapon.firePointsAvaiable;


                //Aplicar los cambios del PowerUp al componente Weapon
                weapon.bulletForce = powerUpData.bulletForce;
                weapon.fireRate = powerUpData.fireRate;
                weapon.bulletsPerBurst = powerUpData.bulletsPerBurst;
                weapon.firePoints = GetFirePointsFromIndices(originalFirePointAvaiable, powerUpData.firePointIndices);


                if (revertChanges == null)
                {
                    //Programar la reversión de los cambios después del tiempo determinado


                    revertChanges = StartCoroutine(RevertChanges(weapon, originalBulletForce, originalFireRate, originalBulletsPerBurst, originalFirePoints));

                }
                //Desactivar MeshRenderder del PowerUp
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }
        }
    }

    private IEnumerator RevertChanges(Weapon weapon, float originalBulletForce, float originalFireRate, int originalBulletsPerBurst, Transform[] originalFirePoints)
    {
        float elapsedTime = 0f;

        while (elapsedTime < powerUpDuration)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        //Revertir los cambios en el componente Weapon
        weapon.bulletForce = originalBulletForce;
        weapon.fireRate = originalFireRate;
        weapon.bulletsPerBurst = originalBulletsPerBurst;
        weapon.firePoints = originalFirePoints;


        Destroy(gameObject);
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
