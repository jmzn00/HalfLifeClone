using System;
using TMPro;
using UnityEngine;

public class Enemy_Teddy : EnemyAi
{
    [SerializeField] private TMP_Text stateText;

    [Header("Colliders")]
    [SerializeField] private GameObject hitbox;

    public override void Awake()
    {
        base.Awake();        
    }
    public override void Update()
    {        
        base.Update();
        stateText.transform.forward = Camera.main.transform.forward;
    }
    public override void ReportCanSee(DetectableTarget target)
    {
        base.ReportCanSee(target);          
    }
    public override void ReportLostSight(DetectableTarget target)
    {
        base.ReportLostSight(target);             
    }
    public override void ChangeState(AiState state)
    {
        base.ChangeState(state);
        stateText.text = currentState.stateName;
    }
    public override void ToggleColliders(bool value)
    {
        hitbox.SetActive(value);
    }
}

