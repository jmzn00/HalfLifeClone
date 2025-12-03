using UnityEngine;

[CreateAssetMenu(menuName = "Ai/Transitions/LeapToDeath")]
public class AiTransition_StateToDeath : AiTransition
{
    public AiState deathState;
    public override AiState Check(EnemyAi controller)
    {
        if(controller.Damageable.Health <= 0f && deathState != null) 
        {
            return deathState;
        }
        return null;
    }
}
