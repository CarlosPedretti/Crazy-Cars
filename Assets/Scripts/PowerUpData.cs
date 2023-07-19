using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp Data", menuName = "PowerUp Data")]
public class PowerUpData : ScriptableObject
{
    public float bulletForce = 20f;
    public float fireRate = 0.1f;
    public int bulletsPerBurst = 3;

    public int quantityOfMines;

    public float maxHeatLevel = 400f;
    public float heatIncreasePerShot = 10f;
    public float heatDecreaseRate = 45f;

    public List<int> firePointIndices = new List<int>();



}



