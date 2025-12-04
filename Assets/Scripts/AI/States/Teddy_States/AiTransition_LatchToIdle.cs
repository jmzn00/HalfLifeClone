using UnityEngine;

[CreateAssetMenu(menuName = "Ai/Transitions/LatchToState")]
public class AiTransition_LatchToIdle : AiTransition
{
    public AiState state;
    public float attackCooldown = 3f;
    public override AiState Check(EnemyAi controller)
    {
        if (controller.isLatched)
            return null;
        else 
        {
            controller.attackCooldownTimer = attackCooldown;
            return state;
        }
        
    }
}
