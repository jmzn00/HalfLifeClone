using UnityEngine;

public class GameEndActivatable : IActivatable
{
    public void Activate() 
    {
        GameServices.GameManager.GameEnded(true, true);
    }
    public void Deactivate() 
    {
    
    }
    public void Toggle() 
    {
    
    }
}
