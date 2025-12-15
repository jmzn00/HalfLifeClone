using UnityEngine;
public enum WeaponType 
{
    Hitscan,
    Projectile,
    Melee
}
public enum AmmoType 
{   
    A_9mm,
    A_357,
    A_45,
    Default
}
public enum WeaponColumn 
{
    Melee = 1,
    Pistols = 2,
    Shotguns = 3,
    Rifle = 4,
    Explosives = 5
}
[CreateAssetMenu(menuName = "Weapons/Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("Description / Visuals")]    
    public GameObject mesh;
    public GameObject worldModel;
    public WeaponEffects weaponEffects;
    public WeaponType weaponType;
    public bool isUnlocked = false;
    

    [Header("UI")]
    public string weaponName;
    public Sprite weaponIcon;
    public WeaponColumn weaponColumn;
    public int rowInColumn;

    [Header("Animation")]
    // this is a set of hand animations for the weapon
    // ie reload, equip, fire etc
    public HandAnimationSet handAnimationSet; 

    // animation clips fot the weapon model
    public AnimationClip equipAnim;
    public AnimationClip reloadAnim;
    public AnimationClip fireAnim;
    public AnimationClip emptyAnim;

    [Header("Stats")]
    public float baseDamage;
    public float fireRate;
    public int magazineSize;
    public int maxAmmoCapacity;
    [Space]
    public bool isAutomatic;    
    public float dropOffDistance;
    public int projectileAmount;
    public AmmoType ammoType;

    [Header("Debuffs")]
    public int shotsBeforeDebuff;
    public float debuffSpreadAngle;
    public float consequtiveWindow = 0.4f;
}
