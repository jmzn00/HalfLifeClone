using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/Death")]
    public class Condition_Dead : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.Damageable.Health <= 0f;
        }
    }
}