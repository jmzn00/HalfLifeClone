using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class DetectableTargetManager : MonoBehaviour
{
    private List<DetectableTarget> _detectableTargets = new();
    public List<DetectableTarget> DetectableTargets => _detectableTargets;
    private void Awake()
    {
        if(GameServices.DetectableTargetManager != this)
            GameServices.DetectableTargetManager = this;
    }
    private void OnDisable()
    {
        if(GameServices.DetectableTargetManager == this)
            GameServices.DetectableTargetManager = null;
    }

    public void RegisterDetectable(DetectableTarget target) 
    {
        if (_detectableTargets.Contains(target)) 
        {
            Debug.LogWarning("Trying to assign a target multiple times");
            return;
        }
        _detectableTargets.Add(target);
    }
    public void UnregisterDetectable(DetectableTarget target) 
    {
        if (!_detectableTargets.Contains(target)) 
        {
            Debug.LogWarning("Trying to unregister a non existant target");
            return;
        }
        _detectableTargets.Remove(target);
    }
}
