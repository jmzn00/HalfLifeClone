using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/New/NoCurrentTarget")]
    public class Condition_NoCurrentTarget : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.CurrentTarget == null;
        }
    }
}