using UnityEngine;
public enum HitboxType 
{
    Head, Body, Legs, Arms
}
public class Hitbox : MonoBehaviour
{
    public HitboxType hitboxType;

    [Header("Flags")]
    public bool meleeImmune;
    public bool hitImmune;

    IDamageabale damageable = null;

    private void Awake()
    {
        damageable = GetComponentInParent<IDamageabale>();
    }

    public HitOutcome ForwardHit(HitInfo info) 
    {
        if(hitImmune || meleeImmune && info.isMelee) return new HitOutcome 
        {
            result = HitResult.Immune,
            damageApplied = 0,
            impactPoint = info.point
        };

        return damageable.ApplyHit(in info);
    }
}
