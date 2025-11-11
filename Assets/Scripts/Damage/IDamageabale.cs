using UnityEngine;

public interface IDamageabale
{
    HitOutcome ApplyHit(in HitInfo hitInfo);
}
