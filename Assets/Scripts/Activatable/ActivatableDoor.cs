using UnityEngine;
using UnityEngine.AI;

public class ActivatableDoor : MonoBehaviour, IActivatable
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private NavMeshObstacle NavMeshObstacle;

    private bool isOpen = false;

    private void Awake()
    {
        Deactivate();
    }
    public void Activate() 
    {
        if (!isOpen) 
        {
            doorAnimator.SetTrigger("Open");
            NavMeshObstacle.enabled = false;
            isOpen = true;
        }                           
    }
    public void Deactivate() 
    {
        if (isOpen) 
        {
            doorAnimator.SetTrigger("Close");
            NavMeshObstacle.enabled = true;
            isOpen = false;
        }       
    }
    public void Toggle() 
    {
    
    }
}
