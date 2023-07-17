using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public GameObject hitPrefab;
    public float hitPrefabDestroyTime;


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

            Quaternion hitRotation = Quaternion.LookRotation(-transform.forward);
            GameObject hitInstance = Instantiate(hitPrefab, transform.position, hitRotation);
            Destroy(hitInstance, hitPrefabDestroyTime);
        }

        Destroy(gameObject);
    }


}

