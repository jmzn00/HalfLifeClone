using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageabale
{
    private void OnEnable()
    {
        if(GameServices.PlayerHealth != this)
            GameServices.PlayerHealth = this;
    }
    private void OnDisable()
    {
        if(GameServices.PlayerHealth == this)
            GameServices.PlayerHealth = null;
    }
    public HitOutcome ApplyHit(in HitInfo hitInfo)
    {
        Debug.Log("Player was attacked for" + hitInfo.baseDamage);

        return new HitOutcome
        {
            result = HitResult.Normal,
            damageApplied = hitInfo.baseDamage,
            impactPoint = hitInfo.point,
            hitbox = hitInfo.hitbox,
            lethalDamage = false
        };
    }
}
