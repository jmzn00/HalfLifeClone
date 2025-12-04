using UnityEditor.Compilation;
using UnityEngine;

public abstract class AiAction : ScriptableObject 
{
    public abstract void Act(EnemyAi controller);     
}
public abstract class AiTransition : ScriptableObject
{
    public abstract AiState Check(EnemyAi controller);
}
