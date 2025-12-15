using Assets.Scripts.AI;
using UnityEngine;

public abstract class AiAction : ScriptableObject 
{
    public AiAudio AiAudio;
    public abstract void Act(EnemyAi controller);   
    public virtual void OnEnter(EnemyAi controller) 
    {
        if (AiAudio)
            AiAudio.OnEnter(controller);
    }
    public virtual void OnExit(EnemyAi controller) 
    {
        if (AiAudio)
            AiAudio.OnExit(controller);
    }
}
public abstract class AiTransition : ScriptableObject
{
    [Header("Transition Conditions")]
    public AiTransitionCondition[] conditions;

    protected bool ConditionsMet(EnemyAi controller)
    {
        if(conditions == null || conditions.Length == 0)
            return true;
        foreach(var condition in conditions) 
        {
            if (condition == null) continue;
            if (!condition.CheckCondition(controller))
            {
                return false;
            }
        }
        return true;
    }
    public abstract AiState Check(EnemyAi controller);
}
