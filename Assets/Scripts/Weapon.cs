using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform[] firePoints; // Puntos de origen del disparo
    public Transform[] firePointsAvaiable;
    public GameObject bulletPrefab; // Prefabricado de la bala
    public float bulletForce = 20f; // Fuerza de disparo de la bala
    public float fireRate = 0.1f; // Tasa de disparo (balas por segundo)
    public int bulletsPerBurst = 3; // Cantidad de balas por r�faga
    public float burstInterval = 0.2f; // Intervalo entre r�fagas

    private float nextFireTime; // Tiempo del siguiente disparo
    private bool isFiring; // Indicador de si se est� disparando

    private void Start()
    {
        nextFireTime = 0f; // Inicializar el tiempo del siguiente disparo a cero
        isFiring = false; // Inicializar el estado de disparo a falso
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isFiring) // Si el jugador presiona el bot�n de disparo y no est� disparando actualmente
        {
            isFiring = true; // Activar el estado de disparo
            nextFireTime = Time.time; // Establecer el tiempo del siguiente disparo al tiempo actual
            FireBurst(); // Iniciar r�faga de disparos
        }

        if (Input.GetButtonUp("Fire1")) // Si el jugador suelta el bot�n de disparo
        {
            isFiring = false; // Desactivar el estado de disparo
        }
    }

    private void FireBurst()
    {
        for (int i = 0; i < bulletsPerBurst; i++) // Disparar la cantidad de balas por r�faga
        {
            for (int j = 0; j < firePoints.Length; j++) // Disparar desde cada punto de origen del disparo
            {
                Shoot(firePoints[j]);
            }
        }

        if (isFiring) // Si a�n se est� manteniendo presionado el bot�n de disparo
        {
            Invoke("FireBurst", burstInterval); // Esperar el intervalo entre r�fagas y disparar nuevamente
        }
    }

    public void Shoot(Transform firePoint)
    {
        // Instanciar una bala en el firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Obtener el componente Rigidbody de la bala
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            // Aplicar fuerza a la bala en la direcci�n del firePoint
            bulletRigidbody.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        }

        Destroy(bullet, 15f);
    }
}