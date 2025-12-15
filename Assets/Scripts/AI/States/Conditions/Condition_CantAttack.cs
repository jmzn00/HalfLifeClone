using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/New/CantAttack")]
    public class Condition_CantAttack : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return !controller.CanAttack();
        }
    }
}