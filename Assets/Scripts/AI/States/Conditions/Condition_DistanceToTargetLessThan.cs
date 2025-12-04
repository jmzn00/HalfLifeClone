using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/DistanceToTargetLessThan")]
    public class Condition_DistanceToTargetLessThan : AiTransitionCondition
    {
        public float distanceThreshold;
        public override bool CheckCondition(EnemyAi controller)
        {
            if (controller.CurrentTarget == null)
                return false;
            if(Vector3.Distance(controller.EyeLocation.position, Camera.main.transform.position + Camera.main.transform.forward) < distanceThreshold) 
            {
                return true;
            }
            return false;
        }
    }
}