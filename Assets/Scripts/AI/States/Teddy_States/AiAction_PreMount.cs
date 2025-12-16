using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Teddy_States
{
    [CreateAssetMenu(menuName = "Ai/Actions/PreMounted")]
    public class AiAction_PreMount : AiAction
    {
        public override void OnEnter(EnemyAi controller)
        {
            base.OnEnter(controller);

            controller.SetAnimTrigger("Mount");
        }
        public override void OnExit(EnemyAi controller) 
        {
            base.OnExit(controller);
        }
        public override void Act(EnemyAi controller)
        {
            
        }
    }
}