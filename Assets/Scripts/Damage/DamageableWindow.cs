using UnityEngine;

public class DamageableWindow : MonoBehaviour, IDamageabale
{
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private FractureGlass fractureGlass;
    private float currentHealth;
    private BoxCollider col;
    private void Awake()
    {
        pool = GameServices.Pool;
        col = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public HitOutcome ApplyHit(in HitInfo info)
    {
        HitOutcome outcome = new HitOutcome
        {
            damageApplied = info.baseDamage,
            impactPoint = info.point,
        };
        HealthChanged(-outcome.damageApplied);
        HandleDamageText(outcome);
        return outcome;
    }
    private void HealthChanged(float value) 
    {
        currentHealth += value;
        if(currentHealth <= 0) 
        {
            fractureGlass?.Fracture();
            col.isTrigger = true;
        }
    }
    [SerializeField] private Transform damageTextSpawnPoint;
    private Pool pool;
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
