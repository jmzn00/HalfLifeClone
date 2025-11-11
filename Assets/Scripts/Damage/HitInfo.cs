using UnityEngine;

public enum HitResult { Normal, Critical, Killed, Immune}

public struct HitInfo 
{
    public Vector3 point;
    public Vector3 normal;

    public bool isMelee;

    public float baseDamage;
    public HitboxType hitbox;
}

public struct HitOutcome 
{
    public HitResult result;
    public HitboxType hitbox;
    public float damageApplied;
    public Vector3 impactPoint;

    public string sfxTag;
    public string vfxTag;
}
