using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/CooldownLessThan")]
    public class Condition_CooldownLessThan : AiTransitionCondition
    {
        public float amount = 0.1f;
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.attackCooldownTimer < amount;
        }
    }
}