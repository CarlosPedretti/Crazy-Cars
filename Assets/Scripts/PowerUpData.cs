using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp Data", menuName = "PowerUp Data")]
public class PowerUpData : ScriptableObject
{
    public float bulletForce = 20f;
    public float fireRate = 0.1f;
    public int bulletsPerBurst = 3;
    public List<int> firePointIndices = new List<int>(); // Índices de los transforms de los puntos de origen del disparo

    // Agrega más variables según tus necesidades
}



