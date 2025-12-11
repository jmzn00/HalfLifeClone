using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.Settings
{
    [CreateAssetMenu(menuName = "Ai/Settings/LockTargetPlayer")]
    public class AiStateSetting_LockTargetPlayer : AiStateSetting
    {
        public override void OnEnter(EnemyAi controller)
        {
            DetectableTarget player = GameServices.Player.DetectableTarget;

            controller.SetTargetLock(true);
            controller.SetTarget(player);
        }
        public override void OnExit(EnemyAi controller) 
        {
            
        }
    }
}