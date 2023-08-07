using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public Vector3 respawnPosition;
    public Quaternion respawnRotation;

    void Start()
    {
       
        if (respawnPosition == Vector3.zero)
        {
            respawnPosition = transform.position;
        }


        if (respawnRotation == Quaternion.identity)
        {
            respawnRotation = transform.rotation;
        }
    }
}