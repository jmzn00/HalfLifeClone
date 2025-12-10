using System;
using UnityEngine;

public class NpcDamageable : MonoBehaviour, IDamageabale
{
    [SerializeField] private Transform damageTextSpawnPoint;
    [SerializeField] private float maxHealth = 100f;
    public float Health { get; private set; }
    public bool Dead;

    [Header("Multipliers")]
    [SerializeField] private float headMultiplier = 1.5f;
    [SerializeField] private float bodyMultiplier = 1f;
    [SerializeField] private float legMultiplier = 0.8f;
    [SerializeField] private float armMultiplier = 0.8f;

    public Action<float> OnHealthChanged;

    private Pool pool;    
    private void Awake()
    {
        pool = GameServices.Pool;
        
    }
    private void OnEnable()
    {
        Health = maxHealth;
        Dead = false;
    }
    private void OnDisable()
    {
        
    }
    public HitOutcome ApplyHit(in HitInfo hitInfo) 
    {
        Debug.Log(gameObject.name + " Hit " + hitInfo.baseDamage);
        float damage = CalculateDamage(hitInfo);
        Health -= damage;
        Health = Mathf.Clamp(Health, 0, maxHealth);
        OnHealthChanged?.Invoke(Health);

        bool lethal = Health <= 0;

        HitOutcome result = new HitOutcome 
        {
            result = HitResult.Normal,
            damageApplied = damage,
            impactPoint = hitInfo.point,
            hitbox = hitInfo.hitbox,
            lethalDamage = lethal
        };                        
        HandleDamageText(result);
        return result;
        
    }    
    private float CalculateDamage(in HitInfo info) 
    {
        float damage = info.baseDamage;
        switch (info.hitbox) 
        {
            case HitboxType.Head:
                damage *= headMultiplier;
                break;
            case HitboxType.Body:
                damage *= bodyMultiplier;
                break;
            case HitboxType.Arms:
                damage *= armMultiplier;
                break;
            case HitboxType.Legs:
                damage *= armMultiplier;
                break;
        }
        return damage;
    }

    DamageText damageText;
    private void HandleDamageText(in HitOutcome outcome)
    {
        if (damageText != null && damageText.gameObject.activeInHierarchy)
        {
            damageText.UpdateDmg(damageTextSpawnPoint.position, outcome.damageApplied);
        }
        else
            damageText = pool.SpawnDamageText(damageTextSpawnPoint.position, outcome.damageApplied);
    }
}
