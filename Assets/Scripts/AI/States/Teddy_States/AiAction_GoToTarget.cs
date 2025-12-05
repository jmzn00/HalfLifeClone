using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Teddy_States
{
    [CreateAssetMenu(menuName = "Ai/Actions/GoToTarget")]
    public class AiAction_GoToTarget : AiAction
    {
        public override void Act(EnemyAi controller)
        {
            controller.Agent.SetDestination(controller.CurrentTarget.transform.position);
        }
    }
}