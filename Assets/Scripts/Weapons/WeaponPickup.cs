using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private float bobHeight = 0.25f;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private WeaponData weapon;
    [SerializeField] private Transform spawnPos;

    private GameObject weaponInstance;
    private Vector3 startPos;

    private BoxCollider col;
    private void Awake()
    {
        if (weapon == null) return;

        GameObject g = weapon.worldModel;
        if (g == null) g = weapon.mesh;

        weaponInstance = Instantiate(weapon.worldModel, spawnPos.position, spawnPos.rotation);
        weaponInstance.transform.SetParent(spawnPos);
        weaponInstance.transform.localScale = new Vector3(2, 2, 2);

        startPos = weaponInstance.transform.localPosition;
    }
    private void Update()
    {
        if (weaponInstance == null) return;

        float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        weaponInstance.transform.localPosition = startPos + new Vector3(0, yOffset, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (weapon == null) return;

        if (other.CompareTag("Player")) 
        {
            if (GameServices.Player.Weapons.TryAddWeapon(weapon)) 
            {
                Destroy(gameObject);
            }            
        }
    }
    private void OnDrawGizmos()
    {
        if (col == null)
        {
            col = GetComponent<BoxCollider>();
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(col.center, col.size);
        
    }
}
