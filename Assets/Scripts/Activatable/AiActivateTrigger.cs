using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AiActivateTrigger : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private List<EnemyAi> enemyAi;

    private BoxCollider col;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        for (int i = 0; i < enemyAi.Count; i++) 
        {
            if (enemyAi[i] != null)
                enemyAi[i].SetActivated(true);
        }
    }
    private void OnDrawGizmos()
    {        
        if (col == null) 
        {
            col = GetComponent<BoxCollider>();            
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(col.bounds.center, col.bounds.size);
    }
}
