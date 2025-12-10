using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/New/AiActivated")]
    public class Condition_AiActivated : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.Activated;
        }
    }
}