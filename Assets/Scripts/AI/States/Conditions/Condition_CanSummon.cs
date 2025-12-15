using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/CanSummon")]
    public class Condition_CanSummon : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.summonCooldownTimer <= 0f;
        }
    }
}