using UnityEngine;

public class NpcDamageable : MonoBehaviour, IDamageabale
{
    public HitOutcome ApplyHit(in HitInfo hitInfo) 
    {
        HitOutcome result = new HitOutcome 
        {
            result = HitResult.Normal,
            damageApplied = hitInfo.baseDamage,
            impactPoint = hitInfo.point,
            hitbox = hitInfo.hitbox
        };
        Debug.Log($"{gameObject.name} hit for {result.damageApplied} hb: {result.hitbox}");

        return result;
        
    }
}
