using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/New/AgentEnabled")]
    public class Condition_AgentEnabled : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.Agent.enabled;
        }
    }
}