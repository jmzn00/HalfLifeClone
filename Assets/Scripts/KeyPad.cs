using UnityEngine;

public class KeyPad : ActivatorBase
{
    [SerializeField] private UnlockDoorModule unlockDoorModule;
    public bool TryUnlock(string keyId) 
    {
        if (keyId == unlockDoorModule.keyId) 
        {
            TriggerActivation();
            return true;
        }
        return false;
    }
}
