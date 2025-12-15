using UnityEngine;

[CreateAssetMenu(menuName = "Ai/Conditions/Condition")]
public abstract class AiTransitionCondition : ScriptableObject
{
    public abstract bool CheckCondition(EnemyAi controller);
}
