using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void AmmoChangedEvent(int ammoInMag, int ammoInReserve);

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponHoldPoint;
    [SerializeField] private List<WeaponData> unlockedWeapons = new List<WeaponData>();
    private List<WeaponRuntime> weaponRuntimes = new List<WeaponRuntime>();

    private WeaponData currentWeapon; 
    private WeaponRuntime currentWeaponRuntime;

    

    public bool attackPending = false;
    

    private int weaponIndex = 0;

    private Camera playerCam;

    private HandAnimController handAnimController;

    private int consecutiveShots = 0;
    private float lastShotTime = -999f;

    public static event AmmoChangedEvent OnAmmoChanged;

    private void Awake()
    {
        playerCam = Camera.main;
        handAnimController = GetComponent<HandAnimController>();

        GameServices.Input.Actions.Player.Attack.performed += ctx => attackPending = true;
        GameServices.Input.Actions.Player.Attack.canceled += ctx => attackPending = false;
        GameServices.Input.Actions.Player.WeaponScroll.performed += ctx => WeaponScroll((int)ctx.ReadValue<float>());                       
        GameServices.Input.Actions.Player.Reload.performed += ctx => Reload();
    }
    private float timer = 0f;

    private bool CanFire() 
    {
        if(!handAnimController.IsAnimationPlaying("Draw") && currentWeaponRuntime.ammoInClip > 0) 
            return true;
        return false;
    }
    private void Update()
    {
        if (!currentWeapon) 
        {
            Debug.LogWarning("No weapon equipped");
            return;
        }
        
        if (attackPending && CanFire() && timer >= currentWeapon.fireRate) 
        {          
            if (currentWeapon.isAutomatic) 
            {
                Attack();                
            }
            else 
            {
                Attack();
                attackPending = false;
            }
            timer = 0f;
        }
        timer += Time.deltaTime;
    }    
    private void Reload() 
    {
        int magazineSize = currentWeaponRuntime.weaponData.magazineSize;
        int reserveAmmo = currentWeaponRuntime.ammoInReserve;

        int currentClipAmmo = currentWeaponRuntime.ammoInClip;
        int neededAmmo = magazineSize - currentClipAmmo;

        
    }
    private void UpdateAmmo(WeaponRuntime weaponRuntime, int amount) 
    {
        weaponRuntime.ammoInClip += amount;
        OnAmmoChanged?.Invoke(weaponRuntime.ammoInClip, weaponRuntime.ammoInReserve);
    }
    public void AddAmmo(AmmoType type, int amount) 
    {           
        for (int i  = 0; i < weaponRuntimes.Count; i++) 
        {
            if (weaponRuntimes[i].weaponData.ammoType == type && weaponRuntimes[i].weaponData.weaponType != WeaponType.Melee) 
            {
                weaponRuntimes[i].ammoInReserve += amount;
                OnAmmoChanged?.Invoke(weaponRuntimes[i].ammoInClip, weaponRuntimes[i].ammoInReserve);
                break;
            }
        }
    }
    private void WeaponScroll(int value) 
    {
        weaponIndex += value;
        weaponIndex = Mathf.Clamp(weaponIndex, 0, unlockedWeapons.Count - 1);

        currentWeapon = unlockedWeapons[weaponIndex];

        ApplyWeapon(currentWeapon);
    }
    private void ApplyWeapon(WeaponData data)
    {
        if (data == null) return;

        handAnimController.ApplyOverride(data.handAnimationSet);
        EquipWeapon(data);
    }
    private void EquipWeapon(WeaponData data)
    {
        if (data == null) return;
        currentWeapon = data;

        currentWeaponRuntime = null;
        for (int i = 0; i < weaponRuntimes.Count; i++) // weapon exists
        {
            if (weaponRuntimes[i].weaponData == data)
            {
                currentWeaponRuntime = weaponRuntimes[i];
                break;
            }
        }

        if (currentWeaponRuntime == null) // weapon doesn't exist yet 
        {
            GameObject weaponInstance = Instantiate(data.mesh, weaponHoldPoint);
            weaponInstance.transform.localPosition = Vector3.zero;
            weaponInstance.transform.localRotation = Quaternion.identity;

            WeaponView weaponView = weaponInstance.GetComponent<WeaponView>();

            ParticleSystem vfxParticle = null;
            if (data.weaponEffects != null && data.weaponEffects.fireVfxPrefab != null)
            {
                GameObject vfxInstance = Instantiate(data.weaponEffects.fireVfxPrefab);
                vfxInstance.transform.SetParent(weaponView.MuzzlePoint, false);
                vfxInstance.transform.localPosition = Vector3.zero;
                vfxInstance.transform.localRotation = Quaternion.identity;
                vfxParticle = vfxInstance.GetComponent<ParticleSystem>();

                if (vfxParticle != null)
                {
                    var main = vfxParticle.main;
                    main.simulationSpace = ParticleSystemSimulationSpace.World;

                    vfxParticle.Stop();
                }
            }

            currentWeaponRuntime = new WeaponRuntime
            {
                weaponData = data,
                weaponInstance = weaponInstance,
                weaponView = weaponView,
                muzzleVfxInstance = vfxParticle,                
            };

            weaponRuntimes.Add(currentWeaponRuntime);
            AddAmmo(data.ammoType, data.magazineSize * 3);
        }
        
        for (int i = 0; i < weaponRuntimes.Count; i++)
        {
            bool active = weaponRuntimes[i].weaponData == data;
            weaponRuntimes[i].weaponInstance.SetActive(active);
        }

        handAnimController.SetTrigger("Draw");
    }

    private void Attack() 
    {
        if (currentWeapon == null) return;

        handAnimController.SetTrigger("Attack");

        switch (currentWeapon.weaponType) 
        {
            case WeaponType.Hitscan:
                HandleHitscan();
                break;
            case WeaponType.Projectile:
                HandleProjectile();
                break;
            case WeaponType.Melee:
                HandleMelee();
                break;
        }
    }
    private Vector3 ApplyConeSpread(Vector3 baseDir, float coneAngDeg) 
    {
        if (coneAngDeg <= 0f) return baseDir;

        float yaw = UnityEngine.Random.Range(-coneAngDeg, coneAngDeg);
        float pitch = UnityEngine.Random.Range(-coneAngDeg, coneAngDeg); 

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        return rot * baseDir;
    }
    private void HandleProjectile() 
    {
        
    }
    private void HandleHitscan() 
    {
        float now = Time.time;
        if (now - lastShotTime <= currentWeaponRuntime.weaponData.consequtiveWindow)        
            consecutiveShots++;        
        else        
            consecutiveShots = 1;

        lastShotTime = now;

        float activeSpread = 0f;
        if (consecutiveShots >= currentWeaponRuntime.weaponData.shotsBeforeDebuff) 
            activeSpread = currentWeaponRuntime.weaponData.debuffSpreadAngle;

        Vector3 origin = playerCam.transform.position;
        Vector3 dir = playerCam.transform.forward;

        if(activeSpread > 0f) 
            dir = ApplyConeSpread(dir, activeSpread);


        //Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        //Debug.Log("Active spread" + activeSpread);
        Debug.DrawRay(origin, dir * 100f, Color.red, 10f);
        if (Physics.Raycast(origin, dir, out RaycastHit hit))
        {
            Hitbox hitbox = hit.collider.GetComponent<Hitbox>();
            if (hitbox)
            {
                HitOutcome outcome = hitbox.ForwardHit(new HitInfo
                {
                    point = hit.point,
                    normal = hit.normal,
                    isMelee = false,
                    baseDamage = currentWeaponRuntime.weaponData.baseDamage,
                    hitbox = hitbox.hitboxType
                });
            }
        }
        currentWeaponRuntime.muzzleVfxInstance.Play();
        UpdateAmmo(currentWeaponRuntime, -1);
    }
    private void HandleMelee() 
    {
        
    }

}
