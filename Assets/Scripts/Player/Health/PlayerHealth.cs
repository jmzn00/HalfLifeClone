using System;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageabale
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    [SerializeField] private float recentlyAttackedCooldown = 2f;
    private float recentlyAttackedTimer = 0f;
    public bool RecentlyAttacked => recentlyAttackedTimer > 0f;

    public static event Action<float> OnHealthChanged;

    private void Start()
    {
        HealthChanged(maxHealth);
    }    
    public bool TryHeal(float amt) 
    {
        if(currentHealth >= maxHealth)
            return false;

        HealthChanged(amt);
        return true;
    }
    private void HealthChanged(float value) 
    {
        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);

        if(currentHealth <= 0f)
            GameServices.GameManager.RestartGame();
    }

    private void Update()
    {
        recentlyAttackedTimer -= Time.deltaTime;
    }

    public HitOutcome ApplyHit(in HitInfo hitInfo)
    {
        recentlyAttackedTimer = recentlyAttackedCooldown;
        HealthChanged(-hitInfo.baseDamage);

        return new HitOutcome
        {
            result = HitResult.Normal,
            damageApplied = hitInfo.baseDamage,
            impactPoint = hitInfo.point,
            hitbox = hitInfo.hitbox,
            lethalDamage = false
        };
    }
}
