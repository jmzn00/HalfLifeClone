using UnityEngine;

[CreateAssetMenu(menuName = "Ai/Conditions/New/IsAttacking")]
public class Condition_IsAttacking : AiTransitionCondition
{
    public override bool CheckCondition(EnemyAi controller)
    {
        if(controller.isAttacking)
            return true;
        return false;
    }
}
