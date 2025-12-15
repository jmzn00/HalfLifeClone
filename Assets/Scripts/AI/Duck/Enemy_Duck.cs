using UnityEngine;

public class Enemy_Duck : EnemyAi
{
    [SerializeField] AiState wanderState;
    public override void Awake()
    {
        base.Awake();
        ChangeState(wanderState);
    }    
    
}
