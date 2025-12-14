using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/IsMounted")]
    public class Condition_IsMounted : AiTransitionCondition
    {
        public override bool CheckCondition(EnemyAi controller)
        {
            return controller.isMounted;
        }
    }
}