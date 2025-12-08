using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageabale
{
    [SerializeField] private float recentlyAttackedCooldown = 2f;
    private float recentlyAttackedTimer = 0f;
    public bool RecentlyAttacked => recentlyAttackedTimer >= 0f;

    private void Update()
    {
        recentlyAttackedTimer -= Time.deltaTime;        
    }
    public HitOutcome ApplyHit(in HitInfo hitInfo)
    {
        recentlyAttackedTimer = recentlyAttackedCooldown;

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
