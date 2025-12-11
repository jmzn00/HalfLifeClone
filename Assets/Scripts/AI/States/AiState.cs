using UnityEngine;
public abstract class AiStateSetting : ScriptableObject 
{
    public virtual void OnEnter(EnemyAi controller) { }
    public virtual void OnExit(EnemyAi controller) { }
}

[CreateAssetMenu(menuName = "Ai/State")]
public class AiState : ScriptableObject
{
    public string stateName;

    public AiAction[] actions;
    public AiTransition[] transitions;
    public AiStateSetting[] settings;

    public virtual void OnEnter(EnemyAi controller) 
    {
        foreach (var a in actions)
            a?.OnEnter(controller);
        foreach (var s in settings)
            s?.OnEnter(controller);
    }
    public virtual void OnExit(EnemyAi controller) 
    {
        foreach(var a in actions)        
            a?.OnExit(controller);
        foreach(var s in settings)
            s?.OnExit(controller);
        
    }
    public void UpdateState(EnemyAi controller) 
    {
        foreach (var a in actions) 
        {
            a.Act(controller);
        }

        foreach (var t in transitions) 
        {
            if (t == null) continue;

            var next = t.Check(controller);
            if(next != null && next != this) 
            {
                controller.ChangeState(next);
                return;
            }
        }
    }

}
