using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public int damage;

    public GameObject explosionPrefab;
    public float explosionPrefabDestroyTime;
    public float mineDestroydelay;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health health = other.gameObject.GetComponent<Health>();

            if (health != null)
            {

                health.TakeDamage(damage); 
            }

            if (explosionPrefab != null)
            {
                GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, transform.rotation);
                Destroy(explosionInstance, explosionPrefabDestroyTime);
            }

            Invoke("DestroyMine", mineDestroydelay);
        }

    }

    private void DestroyMine()
    {
        Destroy(gameObject);
    }


}




