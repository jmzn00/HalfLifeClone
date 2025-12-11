using UnityEngine;

public class ActivatableDoor : MonoBehaviour, IActivatable
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private KeyId keyNeeded;

    private bool isOpen = false;

    private void Awake()
    {
        Deactivate();
    }
    public void Activate() 
    {
        if (GameServices.Player.Inventory.HasKey(keyNeeded)) 
        {
            if (!isOpen) 
            {
                doorAnimator.SetTrigger("Open");
                isOpen = true;
            }            
        }
        
    }
    public void Deactivate() 
    {
        if (isOpen) 
        {
            doorAnimator.SetTrigger("Close");
            isOpen = false;
        }       
    }
    public void Toggle() 
    {
    
    }
}
