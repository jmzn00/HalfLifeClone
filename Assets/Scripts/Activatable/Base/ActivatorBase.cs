using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Activatable 
{
    public GameObject activatableObject;
    public bool activate;
}

public class ActivatorBase : MonoBehaviour
{
    [SerializeField] private Activatable[] activatableObjects;
    protected IActivatable[] activatables;
    protected Dictionary<IActivatable, bool> activatableStates;

    protected virtual void Awake() 
    {
        activatableStates = new Dictionary<IActivatable, bool>(activatableObjects.Length);

        for (int i = 0; i < activatableObjects.Length; i++)
        {
            var entry = activatableObjects[i];

            if (entry.activatableObject == null)
            {
                Debug.LogWarning($"[{name}] Activatable entry {i} has null GameObject");
                continue;
            }

            var ac = entry.activatableObject.GetComponent<IActivatable>();
            if (ac == null)
            {
                Debug.LogWarning($"[{name}] {entry.activatableObject.name} does not have IActivatable");
                continue;
            }

            // Use assignment to avoid duplicate-key exceptions
            activatableStates[ac] = entry.activate;
        }
    }
    protected void TriggerActivation() 
    {
        /*
        foreach (var a in activatables) 
        {
            if(a == null) 
            {
                Debug.LogWarning("Null activateable in list on" + name);
            }
            a.Activate();
        }
        */
        foreach (var kvp in activatableStates)
        {
            var activatable = kvp.Key;
            if (activatable == null)
            {
                Debug.LogWarning($"[{name}] Null IActivatable key (object destroyed?)");
                continue;
            }

            if (kvp.Value) activatable.Activate();
            else activatable.Deactivate();
        }

    }
    protected void TriggerDeactivation() 
    {
        foreach (var kvp in activatableStates)
        {
            kvp.Key?.Deactivate();
        }
    }
}
