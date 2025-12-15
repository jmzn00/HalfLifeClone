using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public class LootEntry 
{
    public string name;
    public GameObject prefab;
    [Range(0f, 1f)]
    public float weight = 1f;
}
public class DestructableBox : MonoBehaviour,  IDamageabale 
{
    [SerializeField] private float maxHealth = 30f;
    private float currentHealth;
    [SerializeField] private List<LootEntry> lootTable = new();
    [SerializeField] private Transform lootSpawnPoint;

    [SerializeField] private GameObject intactObject;
    [SerializeField] private Transform[] cells;
    private List<Rigidbody> _rb = new();
    [SerializeField] private NavMeshObstacle navMeshObstacle;

    bool hasDied;

    private void Awake()
    {
        foreach (var cell in cells)
        {
            if (!cell) continue;

            var rb = cell.GetComponent<Rigidbody>();
            if (!rb) rb = cell.gameObject.AddComponent<Rigidbody>();

            rb.isKinematic = true;          // keep frozen until break
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            _rb.Add(rb);

            cell.gameObject.SetActive(false);
        }        
    }
    private void OnEnable()
    {
        currentHealth = maxHealth;
        hasDied = false;
    }

    public HitOutcome ApplyHit(in HitInfo info) 
    {
        HitOutcome result = new HitOutcome
        {
            damageApplied = info.baseDamage
        };
        HealthChanged(info);
        return result;                    
    }
    private void HealthChanged(in HitInfo info) 
    {
        currentHealth -= info.baseDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if(currentHealth <= 0f) 
        {
            DestroyBox(info.point);
            if (!hasDied) 
            {
                SpawnLoot();
            }
            hasDied = true;
        }
    }
    private void SpawnLoot() 
    {
        if (lootTable == null || lootTable.Count == 0)
            return;

        float totalWeight = 0f;
        foreach (var entry in lootTable) 
        {
            if (entry.weight > 0f)
                totalWeight += entry.weight;
        }
        if (totalWeight <= 0f)
            return;

        float randomValue = Random.value * totalWeight;

        foreach (var entry in lootTable) 
        {
            if (entry.weight <= 0f)
                continue;

            if(randomValue < entry.weight) 
            {
                if (entry.prefab != null) 
                {
                    Vector3 pos = lootSpawnPoint != null ? lootSpawnPoint.position : transform.position;
                    Quaternion rot = lootSpawnPoint != null ? lootSpawnPoint.rotation : transform.rotation;

                    Instantiate(entry.prefab, pos, rot);
                }
                return;
            }
            else 
            {
                randomValue -= entry.weight;
            }
        }
    }
    public float explosionForce = 5f;
    public float explosionRadius = 3f;
    public float upwardsModifier = 1f;
    public float randomSphereRadius = 0.5f;
    private void DestroyBox(Vector3 pos) 
    {
        if(intactObject)
            intactObject.SetActive(false);
        if(navMeshObstacle)
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
}
