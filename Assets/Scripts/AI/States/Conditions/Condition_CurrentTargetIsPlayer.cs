using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/CurrentTargetIsType")]
    public class Condition_CurrentTargetType : AiTransitionCondition
    {
        public DetectableType type;
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.CurrentTarget != null && controller.CurrentTarget.Type == type;
        }
    }
}