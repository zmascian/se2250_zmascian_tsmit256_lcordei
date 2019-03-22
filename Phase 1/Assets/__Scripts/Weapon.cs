using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;                       //Letter to show on power-up
    public Color color = Color.white;           //Color of Collar and power-up
    public GameObject projectilePrefab;         //Prefab for projectiles
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;               //Amount of damage cause
    public float continousDamage = 0;           //Damage per second (laser)
    public float delayBetweenShots = 0;
    public float velocity = 20;                 //Speed of projectiles
}

public class Weapon : MonoBehaviour
{
    
}
