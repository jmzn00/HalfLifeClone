using UnityEngine;

public class DetectableTarget : MonoBehaviour
{
    private void OnEnable()
    {
        GameServices.DetectableTargetManager.RegisterDetectable(this);
    }
    private void OnDisable()
    {
        GameServices.DetectableTargetManager.UnregisterDetectable(this);
    }
}
