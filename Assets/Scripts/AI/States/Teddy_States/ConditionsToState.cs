using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.AI.States.Teddy_States
{
    [CreateAssetMenu(menuName = "Ai/Transitions/ConditionsToState")]
    public class ConditionsToState : AiTransition
    {
        public AiState targetState;

        public override AiState Check(EnemyAi controller)
        {
            Debug.Log($"Checking ConditionsToState {targetState.stateName} {ConditionsMet(controller)}");
            if (ConditionsMet(controller))
            {
                return targetState;
            }
            return null;
        }
    }
}