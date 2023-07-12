using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;


    private void Start()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health health = other.gameObject.GetComponent<Health>();

            if (health != null)
            {
                // El jugador ha colisionado con otro jugador
                // Puedes aplicar alguna lógica aquí, como reducir la vida del jugador
                health.TakeDamage(damage); // Reducir 10 puntos de vida al jugador
            }
        }


        Destroy(gameObject);
    }


}

