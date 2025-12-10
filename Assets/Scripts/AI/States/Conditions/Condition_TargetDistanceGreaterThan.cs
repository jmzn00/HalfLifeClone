using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/TargetDistanceGreaterThan")]
    public class Condition_TargetDistanceGreaterThan : AiTransitionCondition
    {
        public float DistanceThreshold = 5f;
        public override bool CheckCondition(EnemyAi controller)
        {
            if(Vector3.Distance(controller.CurrentTarget.transform.position, controller.transform.position) > DistanceThreshold) 
            {
                return true;
            }
            return false;
        }
    }
}