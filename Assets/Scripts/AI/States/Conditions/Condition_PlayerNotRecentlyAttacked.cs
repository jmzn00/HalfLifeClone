
using UnityEngine;
[CreateAssetMenu(menuName = "Ai/Conditions/PlayerNotRecentlyAttacked")]
public class Condition_PlayerNotRecentlyAttacked : AiTransitionCondition
{
    public override bool CheckCondition(EnemyAi controller)
    {
        if(!GameServices.Player.Health.RecentlyAttacked)
            return true;
        return false;
    }
}
