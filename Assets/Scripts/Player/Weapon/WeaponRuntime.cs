using UnityEngine;

[System.Serializable]
public class WeaponRuntime
{
    public WeaponData weaponData;
    public GameObject weaponInstance;
    public WeaponView weaponView;
    public ParticleSystem muzzleVfxInstance;

    public int ammoInClip;
    public int ammoInReserve;
}
