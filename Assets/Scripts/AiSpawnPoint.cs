using UnityEngine;

public class AiSpawnPoint : MonoBehaviour, IActivatable
{
    [SerializeField] private GameObject aiPrefab;
    [SerializeField] private int amountToSpawn = 3;

    [SerializeField] private AttackSequenceActivator attackSequence;
    public void Activate() 
    {
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
    }
    public void Deactivate() 
    {
    
    }
    public void Toggle() 
    {
    
    }
}
