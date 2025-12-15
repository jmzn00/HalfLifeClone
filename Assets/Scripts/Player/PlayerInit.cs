using System.Runtime.CompilerServices;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class PlayerInit : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private WeaponController playerWeapons;
    private DetectableTarget detectableTarget;
    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerWeapons = GetComponent<WeaponController>();
        detectableTarget = GetComponent<DetectableTarget>();

        GameServices.Player = new Player
        {
            Health = playerHealth,
            Weapons = playerWeapons,
            DetectableTarget = detectableTarget
        };
    }
}
