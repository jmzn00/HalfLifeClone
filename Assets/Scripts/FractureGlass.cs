using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureGlass : MonoBehaviour
{
    private List<Rigidbody> _f = new();
    private void Awake()
    {
        foreach (Transform c in transform) 
        {
            if (c == transform) continue;

            Rigidbody rb = c.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            _f.Add(rb);
        }
    }
    public void Fracture() 
    {
        foreach (var rb in _f) 
        {
            rb.isKinematic = false;
            rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode.Impulse);            
        }
        StartCoroutine(DelayedDestroy());
    }
    IEnumerator DelayedDestroy() 
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
