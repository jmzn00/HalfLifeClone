using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemPickup : MonoBehaviour
{    
    [SerializeField] private Transform meshSpawnPoint;
    [SerializeField] private bool destroyOnPickup = false;
    [SerializeField] private ItemDefinition itemDef;

    [Header("Item Bobbing")]
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 1f;

    private GameObject meshInstance;
    private Vector3 startPos;
    private BoxCollider col;

    private void Awake()
    {
        if (!itemDef.mesh) return;

        meshInstance = Instantiate(itemDef.mesh, meshSpawnPoint);
        meshInstance.transform.localPosition = Vector3.zero;
        meshInstance.transform.localRotation = Quaternion.identity;
        meshInstance.transform.localScale = new Vector3(2, 2, 2);
    }

    private void Update()
    {
        if (meshInstance == null) return;

        float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        meshInstance.transform.localPosition = startPos + new Vector3(0, yOffset, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameServices.Player.Inventory.TryAddItem(itemDef)) 
        {
            if(destroyOnPickup)
                Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (col == null)
            col = GetComponent<BoxCollider>();

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(col.center, col.size);
        Gizmos.matrix = Matrix4x4.identity;

    }
}
