using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
   [CreateAssetMenu(menuName = "Ai/Conditions/DistanceToTargetLessThan")]
    public class Condition_DistanceToTargetLessThan : AiTransitionCondition
    {
        public float distanceThreshold = 2f;
        public override bool CheckCondition(EnemyAi controller)
        {
            return Vector3.Distance(controller.transform.position, controller.CurrentTarget.transform.position) < distanceThreshold;
        }
    }
}