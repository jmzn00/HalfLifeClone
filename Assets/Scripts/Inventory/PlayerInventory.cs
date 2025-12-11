using System.Collections.Generic;
using UnityEngine;
public enum KeyId 
{
    RedKey,
    BlueKey,
    GreenKey,
    BossKey,
    None
}
public class PlayerInventory : MonoBehaviour
{
    private HashSet<KeyId> keys = new HashSet<KeyId>();    

    public void AddKey(KeyId id) 
    {
        if (keys.Add(id))
            Debug.Log($"Picked up key {id}");
    }
    public bool HasKey(KeyId id) 
    {
        return keys.Contains(id);
    }

}
