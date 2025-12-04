using System;
using TMPro;
using UnityEngine;

public class Enemy_Teddy : EnemyAi
{
    private AiState currentState;

    [Header("States")]
    [SerializeField] private AiState idleState;
    [SerializeField] private AiState attackState;

    [SerializeField] private TMP_Text stateText;

    [Header("Colliders")]
    [SerializeField] private GameObject hitbox;

    public override void Awake()
    {
        base.Awake();
        ChangeState(idleState);        
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
        if (Damageable.Dead)
            return;

        base.ReportCanSee(target);       
    }
    public override void ReportLostSight(DetectableTarget target)
    {
        if (Damageable.Dead) 
            return;
        base.ReportLostSight(target);             
    }
    public override void ChangeState(AiState state)
    {
        if (state == null) 
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

