using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/CanAttack")]
    public class Condition_CanAttack : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.CanAttack();
        }
    }
}