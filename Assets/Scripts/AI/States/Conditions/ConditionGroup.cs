using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Conditions
{
    [CreateAssetMenu(menuName = "Ai/Conditions/Group")]
    public class ConditionGroup : AiTransitionCondition
    {
        public enum Operator
        {
            AND,
            OR
        }
        public Operator groupOperator = Operator.AND;
        public AiTransitionCondition[] conditions;

        public override bool CheckCondition(EnemyAi controller)
        {
            if (conditions == null || conditions.Length == 0)
                return true;

            switch (groupOperator) 
            {
                case Operator.AND:
                    foreach (var c in conditions) 
                    {
                        if(c != null && !c.CheckCondition(controller))
                            return false;
                    }
                return true;
                case Operator.OR:
                    foreach (var c in conditions) 
                    {
                        if(c != null && c.CheckCondition(controller))
                            return true;
                    }
                    return false;
            }
            return false;
        }
    }
}