using UnityEngine;

[CreateAssetMenu(menuName = "Ai/Conditions/NotAttacking")]
public class Condition_NotAttacking : AiTransitionCondition
{
    public override bool CheckCondition(EnemyAi controller)
    {
        if(!controller.isAttacking)
            return true;
        return false;
    }
}
