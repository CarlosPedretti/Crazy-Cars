using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public Vector3 respawnPosition;
    public Quaternion respawnRotation; // Variable para almacenar la rotaci�n del objeto al momento de su instancia

    void Start()
    {
        // Si la respawnPosition no ha sido configurada, la establecemos como la posici�n actual del objeto
        if (respawnPosition == Vector3.zero)
        {
            respawnPosition = transform.position;
        }

        // Si la respawnRotation no ha sido configurada, la establecemos como la rotaci�n actual del objeto
        if (respawnRotation == Quaternion.identity)
        {
            respawnRotation = transform.rotation;
        }
    }
}