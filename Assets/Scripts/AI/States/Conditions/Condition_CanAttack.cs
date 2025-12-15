using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/New/CanAttack")]
    public class Condition_CanAttack : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            if (controller.isAttacking || controller.attackCooldownTimer > 0f) 
            {
                return false;
            }
            return true;
        }
    }
}