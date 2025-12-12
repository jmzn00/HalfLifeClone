using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class InventoryPresenter : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private PlayerInventory inventory;

    private GameObject equippedGo;
    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        inventory.OnEquippedChanged += HandleEquipped;
    }
    private void OnDestroy()
    {
        inventory.OnEquippedChanged -= HandleEquipped;
    }
    private void HandleEquipped(ItemInstance inst) 
    {
        if(equippedGo) Destroy(equippedGo);
        if (inst == null) return;

        if (inst.def == null || inst.def.mesh == null)
            return;

        equippedGo = Instantiate(inst.def.mesh, spawnPoint);
        equippedGo.transform.localPosition = Vector3.zero;
        equippedGo.transform.localRotation = Quaternion.identity;
    }    
}
