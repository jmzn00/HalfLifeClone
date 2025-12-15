using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class AiManager : MonoBehaviour
{
    private List<EnemyAi> _allAi = new();
    private void Awake()
    {
        if(GameServices.AiManager != this)
            GameServices.AiManager = this;
    }
    private void OnDestroy()
    {
        if (GameServices.AiManager == this)
            GameServices.AiManager = null;
    }
    public void Register(EnemyAi ai) 
    {
        _allAi.RemoveAll(x => x == null);
        if (ai != null && !_allAi.Contains(ai))
            _allAi.Add(ai);
    }
    public void Unregister(EnemyAi ai) 
    {
        if (ai == null) return;
            _allAi.Remove(ai);
    }
}
