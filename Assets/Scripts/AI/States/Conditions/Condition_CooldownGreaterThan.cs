using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/CooldownGreaterThan")]
    public class Condition_CooldownGreaterThan : AiTransitionCondition
    {
        public float value = 0f;
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.attackCooldownTimer > value;
        }
    }
}