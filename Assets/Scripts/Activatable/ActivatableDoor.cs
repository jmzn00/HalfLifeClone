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
        Debug.Log("Activate");
        if (!isOpen) 
        {            
            doorAnimator.SetTrigger("Open");
            NavMeshObstacle.enabled = false;
            isOpen = true;
            Debug.Log("Open" + gameObject.name + " IsOpen " + isOpen);
        }                           
    }
    public void Deactivate() 
    {
        Debug.Log("Deactivate");
        if (isOpen) 
        {
            doorAnimator.SetTrigger("Close");
            NavMeshObstacle.enabled = true;
            isOpen = false;
            Debug.Log("Close " + gameObject.name + " IsOpen " + isOpen);
        }        
    }
    public void Toggle() 
    {
    
    }
}
