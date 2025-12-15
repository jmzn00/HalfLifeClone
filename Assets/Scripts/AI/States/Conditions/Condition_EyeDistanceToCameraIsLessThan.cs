using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/New/EyeDistanceToCameraLessThan")]
    public class Condition_EyeDistanceToCameraIsLessThan : AiTransitionCondition
    {
        public float DistanceThreshold = 1f;

        public override bool CheckCondition(EnemyAi controller)
        {
            Vector3 target = Camera.main.transform.position;
            Vector3 enemy = controller.EyeLocation.position + controller.EyeLocation.forward;

            float dist = Vector3.Distance(enemy, target);

            if(dist <= DistanceThreshold) 
            {
                return true;
            }
            return false;
        }
    }
}