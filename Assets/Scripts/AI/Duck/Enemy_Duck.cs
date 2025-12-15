using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Duck : EnemyAi
{
    [SerializeField] private TMP_Text stateText;

    [SerializeField] private Collider[] colliders;
    public override void Awake()
    {
        base.Awake();
    }   
    public override void Update()
    {
        base.Update();
        stateText.transform.forward = Camera.main.transform.forward;        
    }

    public override void ChangeState(AiState newState)
    {
        base.ChangeState(newState);
        stateText.text = newState.stateName;
    }
    public override void ToggleColliders(bool value)
    {
        base.ToggleColliders(value);
        foreach (Collider collider in colliders)
            collider.enabled = value;
    }

}
