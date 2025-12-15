using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Teddy_States
{
    [CreateAssetMenu(menuName = "Ai/Actions/LockTargetPlayer")]
    public class AiAction_LockTargetPlayer : AiAction
    {
        public override void OnEnter(EnemyAi controller)
        {
            controller.SetTargetLock(true);
            controller.SetTarget(GameServices.Player.DetectableTarget);
        }
        public override void Act(EnemyAi controller)
        {
            return;
        }
    }
}