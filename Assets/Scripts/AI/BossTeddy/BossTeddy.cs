using TMPro;
using UnityEngine;

public class BossTeddy : EnemyAi
{
    [SerializeField] private TMP_Text stateText;

    public override void Update()
    {
        base.Update();
        if (stateText)
            stateText.transform.forward = Camera.main.transform.forward;
    }

    public override void ChangeState(AiState newState)
    {
        base.ChangeState(newState);

        if(stateText)
            stateText.text = newState.stateName;
    }
    [SerializeField] private Collider[] colliders;
    public override void ToggleColliders(bool value)
    {
        foreach(Collider collider in colliders) 
        {
            if(collider != null)
                collider.enabled = value;
        }
    }
}
