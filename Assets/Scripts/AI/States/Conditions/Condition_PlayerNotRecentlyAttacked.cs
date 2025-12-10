
using UnityEngine;
[CreateAssetMenu(menuName = "Ai/Conditions/New/PlayerNotRecentlyAttacked")]
public class Condition_PlayerNotRecentlyAttacked : AiTransitionCondition
{
    public override bool CheckCondition(EnemyAi controller)
    {
        return !GameServices.Player.Health.RecentlyAttacked;
    }
}
