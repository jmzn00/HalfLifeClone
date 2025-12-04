using UnityEngine;

[CreateAssetMenu(menuName = "Ai/Conditions/PlayerRecentlyAttacked")]
public class Condition_PlayerRecentlyAttacked : AiTransitionCondition
{
    public override bool CheckCondition(EnemyAi controller)
    {
        if(GameServices.Player.Health.RecentlyAttacked)
            return true;
        return false;
    }
}
