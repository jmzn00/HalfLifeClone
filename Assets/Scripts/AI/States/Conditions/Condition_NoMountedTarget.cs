using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/NoMountedTarget")]
    public class Condition_NoMountedTarget : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            if(controller.CurrentTarget != null && controller.CurrentTarget.Type == DetectableType.AiMountable) 
            {
                if(controller.CurrentTarget.LinkedAi.hasMountedTarget)
                    return false;
                return true;
            }
            return false;
        }
    }
}