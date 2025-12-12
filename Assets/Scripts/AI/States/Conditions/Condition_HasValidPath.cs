using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/New/HasValidPathToTarget")]
    public class Condition_HasValidPath : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            if(controller.CurrentTarget == null)
                return false;
            return NavChecks.HasValidPath(controller.transform.position, controller.CurrentTarget.transform.position);
        }
    }
}