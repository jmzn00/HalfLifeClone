using UnityEngine;

public class ActivatorBase : MonoBehaviour
{
    [SerializeField] private GameObject[] activatableObjects;
    protected IActivatable[] activatables;

    protected virtual void Awake() 
    {
        activatables = new IActivatable[activatableObjects.Length];

        for (int i = 0; i < activatableObjects.Length; i++) 
        {
            var go = activatableObjects[i];

            if (go == null) 
            {
                Debug.LogWarning("Null Obj");
                return;
            }

            var a = go.GetComponent<IActivatable>();
            if (a == null) 
            {
                Debug.LogWarning($"{go.name} does not have IActivatable");
                return;
            }
            activatables[i] = a;
        }
    }
    protected void TriggerActivation() 
    {
        foreach (var a in activatables) 
        {
            if(a == null) 
            {
                Debug.LogWarning("Null activateable in list on" + name);
            }
            a.Activate();
        }
        
    }
    protected void TriggerDeactivation() 
    {
        foreach (var a in activatables)
            a?.Deactivate();
    }
}
