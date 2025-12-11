using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerActivator : ActivatorBase
{
    [SerializeField] private bool deactivateOnLeave = false;
    [SerializeField] private bool deactivationOnEnter = false;
    BoxCollider col;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (deactivationOnEnter)
            TriggerDeactivation();
        else
            TriggerActivation();
        
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && deactivateOnLeave) 
        {
            TriggerDeactivation();
        }
        
    }
    private void OnDrawGizmos()
    {        
        if (col == null)
            col = GetComponent<BoxCollider>();
        
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(col.center, col.size);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
