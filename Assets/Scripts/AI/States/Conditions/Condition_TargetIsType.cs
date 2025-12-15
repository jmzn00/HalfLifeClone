using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/New/TargetIsType")]
    public class Condition_TargetIsType : AiTransitionCondition
    {
        public DetectableType targetType;
        public override bool CheckCondition(EnemyAi controller)
        {
            bool y = controller.CurrentTarget.Type == targetType;            
            if(controller.CurrentTarget.Type == targetType)
                return true;
            return false;
        }
    }
}