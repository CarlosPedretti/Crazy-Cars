using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public GameObject hitPrefab;
    public float hitPrefabDestroyTime;

    private int shooterIndex;
    private GameManager gameManager; // Referencia al GameManager

    public void SetShooter(int playerIndex, GameManager gm)
    {
        shooterIndex = playerIndex;
        gameManager = gm;
        Debug.Log("Shooter Index: " + shooterIndex); // Agregamos un Debug.Log para verificar el playerIndex
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health health = other.gameObject.GetComponent<Health>();

            if (health != null)
            {
                // Reducir la salud del jugador golpeado
                health.TakeDamage(damage);

                // Verificar si el jugador ha sido eliminado (su salud es menor o igual a cero)
                if (health.GetCurrentHealth() <= 0)
                {
                    // Verificar si se estableció el índice del jugador que disparó la bala
                    if (shooterIndex >= 0)
                    {
                        // Aumentar 10 puntos al jugador asesino
                        if (gameManager != null)
                        {
                            gameManager.IncreaseScore(shooterIndex);
                            Debug.Log("Score increased for Player " + shooterIndex); // Agregamos un Debug.Log para verificar el aumento del puntaje
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
