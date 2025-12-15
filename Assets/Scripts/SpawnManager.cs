using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private void Awake()
    {
        if(GameServices.SpawnManager != this)
            GameServices.SpawnManager = this;
    }
    private void OnDisable()
    {
        if(GameServices.SpawnManager == this)
            GameServices.SpawnManager = null;
    }
}
