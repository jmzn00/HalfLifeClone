using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/MountableIsDead")]
    public class Condition_MountableIsDead : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.mountedAi != null && controller.mountedAi.Damageable.Dead;
        }
    }
}