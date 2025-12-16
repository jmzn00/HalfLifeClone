using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;



public class AiSpawnPoint : MonoBehaviour, IActivatable
{
    [SerializeField] private GameObject aiPrefab;
    [SerializeField] private int amountToSpawn = 3;

    [SerializeField] private AttackSequenceActivator attackSequence;

    [SerializeField] private List<Summonable> summonables;

    public bool spawned { get; private set; } = false;
    public void Activate() 
    {
        /*
        for (int i = 0; i < amountToSpawn; i++) 
        {
            GameObject go = Instantiate(aiPrefab, transform.position, transform.rotation);

            EnemyAi enemyAi = go.GetComponent<EnemyAi>();
            if (enemyAi) {
                enemyAi.Activate();
                if(attackSequence)
                    attackSequence.AddEnemy(enemyAi);
            }
            
        }
        */
        foreach (var item in summonables) 
        {
            if (item.amount <= 0) continue;
            if (item.prefab == null) continue;

            for (int i = 0; i < item.amount; i++) 
            {
                GameObject go = Instantiate(item.prefab, transform.position, transform.rotation);
                if (attackSequence) 
                {
                    EnemyAi ai = go.GetComponent<EnemyAi>();
                    if(ai != null)
                        attackSequence.AddEnemy(ai);
                }
            }
        }
        spawned = true;
    }
    public void Deactivate() 
    {
    
    }
    public void Toggle() 
    {
    
    }
}
