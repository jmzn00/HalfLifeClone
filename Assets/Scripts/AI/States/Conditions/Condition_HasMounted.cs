using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/HasMounted")]
    public class Condition_HasMounted : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {            
            return controller.hasMountedTarget;            
        }
    }
}