using System.Collections.Generic;
using UnityEngine;
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

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public HitOutcome ApplyHit(in HitInfo info) 
    {
        HitOutcome result = new HitOutcome
        {
            damageApplied = info.baseDamage
        };
        HealthChanged(-result.damageApplied);
        return result;                    
    }
    private void HealthChanged(float value) 
    {
        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if(currentHealth <= 0f) 
        {           
            DestroyBox();
            SpawnLoot();
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
    private void DestroyBox() 
    {
        gameObject.SetActive(false);
    }
}
