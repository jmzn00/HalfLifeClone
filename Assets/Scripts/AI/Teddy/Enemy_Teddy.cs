using System;
using TMPro;
using UnityEngine;

public class Enemy_Teddy : EnemyAi
{
    private AiState currentState;

    [Header("Actions")]
    [SerializeField] private AiState idleState;
    [SerializeField] private AiState attackState;
    [SerializeField] private AiState latchState;
    [SerializeField] private AiState deathState;

    private NpcDamageable damageable;

    [SerializeField] private TMP_Text stateText;

    [Header("Colliders")]
    [SerializeField] private GameObject hitbox;

    public override void Awake()
    {
        base.Awake();
        damageable = GetComponent<NpcDamageable>();
        damageable.OnHealthChanged += HealthChanged;

        ChangeState(idleState);        
    }
    private void HealthChanged(float amt) 
    {
        if(amt <= 0) 
        {
            ChangeState(deathState);
        }
    }
    private bool CanAttack() 
    {
        return !isAttacking && attackCooldownTimer <= 0f;
    }
    private void Update()
    {
        stateText.transform.forward = Camera.main.transform.forward;

        if (currentState != null)
            currentState.UpdateState(this);
        CommonUpdate();
    }
    public override void ReportCanSee(DetectableTarget target)
    {
        if (CanAttack()) 
        {
            base.ReportCanSee(target);
            ChangeState(attackState);
        }        
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
        if(state == null) 
        {
            Debug.LogWarning("State Not Implemented");
            return;
        }
        stateText.text = state.stateName;
        currentState = state;
    }
    public override void ToggleColliders(bool value)
    {
        hitbox.SetActive(value);
    }
}

