using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/HasMountedTarget")]
    public class Condition_HasMountedTarget : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            if (controller.CurrentTarget != null && controller.CurrentTarget.Type == DetectableType.AiMountable)
            {
                if (controller.CurrentTarget.LinkedAi.hasMountedTarget)
                    return true;
                return false;
            }
            return false;
        }
    }
}