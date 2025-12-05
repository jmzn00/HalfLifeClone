using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/TargetIsMountable")]
    public class Condition_CurrentTargetIsMountable : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.CurrentTarget != null && controller.CurrentTarget.Type == DetectableType.AiMountable;
        }
    }
}