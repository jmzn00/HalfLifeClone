using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.Actions
{
    [CreateAssetMenu(menuName = "Ai/Actions/Idle")]
    public class AiAction_Idle : AiAction
    {
        public override void OnEnter(EnemyAi controller)
        {
            controller.SetAnimTrigger("Idle");
        }
        public override void OnExit(EnemyAi controller)
        {
            
        }
        public override void Act(EnemyAi controller)
        {
            
        }
    }
}