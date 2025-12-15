using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/HealthLessThan")]
    public class Condition_HealthLessThan : AiTransitionCondition
    {
        public float amount = 50;
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.Damageable.Health < amount;
        }
    }
}