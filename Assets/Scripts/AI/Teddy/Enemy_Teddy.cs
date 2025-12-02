using UnityEngine;

public class Enemy_Teddy : EnemyAi
{
    private AiState currentState;

    [Header("Actions")]
    [SerializeField] private AiState idleState;
    [SerializeField] private AiState attackState;
    [SerializeField] private AiState latchState;

    public override void Awake()
    {
        base.Awake();

        ChangeState(idleState);
    }
    private void Update()
    {
        if (currentState != null)
            currentState.UpdateState(this);
        CommonUpdate();
    }
    public override void ReportCanSee(DetectableTarget target)
    {
        base.ReportCanSee(target);
        ChangeState(attackState);
    }
    public override void ReportLostSight(DetectableTarget target)
    {
        if (!isAttacking) 
        {
            base.ReportLostSight(target);
            ChangeState(idleState);
        }        
    }
    public override void ChangeState(AiState state)
    {
        currentState = state;
    }        
}
