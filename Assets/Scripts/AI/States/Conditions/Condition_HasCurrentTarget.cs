using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/HasCurrentTarget")]
    public class Condition_HasCurrentTarget : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.CurrentTarget != null;
        }
    }
}