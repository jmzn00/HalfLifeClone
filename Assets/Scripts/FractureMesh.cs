using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureMesh : MonoBehaviour
{
    private List<Rigidbody> _f = new();

    [SerializeField] private MeshRenderer intactMeshRenderer;
    [SerializeField] private Transform[] cells;

    [SerializeField] private float destroyDelay = 2f;
    
    private void Awake()
    {
        if(cells != null && cells.Length > 0) 
        {
            foreach (var cell in cells)
            {
                Rigidbody rb = cell.gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                _f.Add(rb);
            }
            Debug.Log("Cells: " + cells.Length);
            return;
        }
        
        foreach (Transform c in transform) 
        {
            if (c == transform) continue;

            Rigidbody rb = c.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            _f.Add(rb);
        }
    }
    public float explosionForce = 5f;
    public float explosionRadius = 3f;
    public float upwardsModifier = 1f;
    public void Fracture() 
    {        
        if(intactMeshRenderer)
            intactMeshRenderer.enabled = false;

        Vector3 explosionPos = transform.position;
        foreach (var rb in _f) 
        {
            rb.AddExplosionForce
                (
                    explosionForce,
                    explosionPos,
                    explosionRadius,
                    upwardsModifier,
                    ForceMode.Impulse
                );
        }
        StartCoroutine(DelayedDestroy());
    }
    IEnumerator DelayedDestroy() 
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
