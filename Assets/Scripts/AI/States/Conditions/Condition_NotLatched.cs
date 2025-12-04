using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/NotLatched")]
    public class Condition_NotLatched : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return !controller.isLatched;
        }
    }
}