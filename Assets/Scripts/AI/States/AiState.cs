using UnityEngine;

[CreateAssetMenu(menuName = "Ai/State")]
public class AiState : ScriptableObject
{
    public string stateName;

    public AiAction[] actions;
    public AiTransition[] transitions;

    public void UpdateState(EnemyAi controller) 
    {
        foreach (var a in actions) 
        {
            a.Act(controller);
        }

        foreach (var t in transitions) 
        {
            var next = t.Check(controller);
            if(next != null && next != this) 
            {
                controller.ChangeState(next);
                return;
            }
        }
    }

}
