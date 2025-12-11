using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class KeyPickup : MonoBehaviour
{
    [SerializeField] private KeyId keyId;
    [SerializeField] private Transform meshSpawnPoint;
    [SerializeField] private GameObject keyMesh;
    [SerializeField] private bool destroyOnPickup = false;

    private void Awake()
    {
        Instantiate(keyMesh, meshSpawnPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            GameServices.Player.Inventory.AddKey(keyId);
            GameServices.Player.UiController.SendUiMessage($"Picked Up {keyId}");
            if (destroyOnPickup)
                Destroy(gameObject);
        }
    }
}
