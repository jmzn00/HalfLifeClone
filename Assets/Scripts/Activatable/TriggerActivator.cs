using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerActivator : ActivatorBase, IActivatable
{
    [SerializeField] private bool deactivateOnLeave = false;
    [SerializeField] private bool singleActivation = true;
    [SerializeField] private bool activateTriggerOnStart = true;
    private bool hasActivated = false;
    private bool isActivated = false;
    BoxCollider col;
    private void Start()
    {
        isActivated = activateTriggerOnStart;
    }
    public void Activate() 
    {
        isActivated = true;
    }
    public void Deactivate() 
    {
        isActivated = false;
    }
    public void Toggle() 
    {
    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !isActivated) return;
        if (singleActivation) 
        {
            if (hasActivated) return;

            TriggerActivation();
            hasActivated = true;
        }                                
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !isActivated) return;

        if (deactivateOnLeave)
            TriggerDeactivation();        

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
