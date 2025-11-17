using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Effects")]
public class WeaponEffects : ScriptableObject
{
    [Header("Sfx/Vfx Tags")]
    public GameObject fireVfxPrefab;
    public GameObject impactVfxPrefab;
    [Space]
    public AudioClip fireSfx;
    public AudioClip reloadSfx;
    public AudioClip emptySfx;
    public AudioClip equipSfx;
}
