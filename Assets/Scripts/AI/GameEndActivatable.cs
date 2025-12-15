using UnityEngine;

public class GameEndActivatable : MonoBehaviour, IActivatable
{
    public void Activate() 
    {
        Debug.Log("EndGame");
        GameServices.GameManager.GameEnded(true, true);
    }
    public void Deactivate() 
    {
    
    }
    public void Toggle() 
    {
    
    }
}
