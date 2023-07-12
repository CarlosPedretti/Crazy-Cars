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
                health.TakeDamage(damage); 
            }
        }


        Destroy(gameObject);
    }


}

