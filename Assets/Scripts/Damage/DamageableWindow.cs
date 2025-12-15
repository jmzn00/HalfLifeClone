using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DamageableWindow : MonoBehaviour, IDamageabale
{
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;
    private BoxCollider col;

    [SerializeField] private GameObject intactObject;
    [SerializeField] private Transform[] cells;
    private List<Rigidbody> _rb = new();
    [SerializeField] private NavMeshObstacle navMeshObstacle;



    private void Awake()
    {
        pool = GameServices.Pool;
        col = GetComponent<BoxCollider>();

        foreach (var cell in cells)
        {
            if (!cell) continue;

            var rb = cell.GetComponent<Rigidbody>();
            if (!rb) rb = cell.gameObject.AddComponent<Rigidbody>();

            rb.isKinematic = true;          // keep frozen until break
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            _rb.Add(rb);
        }
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
        HealthChanged(info);
        HandleDamageText(outcome);
        return outcome;
    }
    private void HealthChanged(in HitInfo info) 
    {
        currentHealth -= info.baseDamage;
        if(currentHealth <= 0) 
        {
            col.enabled = false;
            DestroyGlass(info.point);
        }
    }
    public float explosionForce = 5f;
    public float explosionRadius = 3f;
    public float upwardsModifier = 1f;
    public float randomSphereRadius = 0.5f;
    private void DestroyGlass(Vector3 pos)
    {
        if (intactObject)
            intactObject.SetActive(false);
        if (navMeshObstacle)
            navMeshObstacle.enabled = false;
        if (_rb.Count <= 0) return;

        Vector3 origin = pos + Random.insideUnitSphere * randomSphereRadius;

        foreach (var entry in _rb)
        {
            entry.gameObject.SetActive(true);
            entry.isKinematic = false;
            entry.AddExplosionForce
                (
                    explosionForce,
                    origin,
                    explosionRadius,
                    upwardsModifier,
                    ForceMode.Impulse
                );
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
