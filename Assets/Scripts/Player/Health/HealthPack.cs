using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HealthPack : MonoBehaviour
{
    [SerializeField] private float healAmount = 15f;
    [SerializeField] private bool destroyOnConsume = true;
    [SerializeField] private GameObject visualMesh;
    [SerializeField] private Transform visualSpawnPoint;
    private BoxCollider col;

    private void Awake()
    {
        var go = Instantiate(visualMesh, visualSpawnPoint);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameServices.Player.Health.TryHeal(healAmount)) 
        {
            if (destroyOnConsume)
                Destroy(gameObject);
        }               
    }
    private void OnDrawGizmos()
    {
        if (col == null)
            col = GetComponent<BoxCollider>();

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(col.center, col.size);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
