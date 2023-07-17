using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public int healValue;
    public float powerUpDestroyDealay;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health health = other.gameObject.GetComponent<Health>();

            if (health != null)
            {

                health.TakeHeal(healValue);
            }


            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer childRenderer in meshRenderers)
            {
                childRenderer.enabled = false;
            }

            Invoke("DestroyPowerUp", powerUpDestroyDealay);


        }

    }

    private void DestroyPowerUp()
    {
        Destroy(gameObject);
    }

}