using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;

public class ActivatableDoor : MonoBehaviour, IActivatable
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private NavMeshObstacle NavMeshObstacle;
    [SerializeField] private bool activateOnAwake = false;

    private bool isOpen = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
            Deactivate();
    }

    private void Awake()
    {
        if(activateOnAwake)
            Activate();
        else
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
