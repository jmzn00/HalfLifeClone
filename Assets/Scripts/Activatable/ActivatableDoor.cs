using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;

public class ActivatableDoor : MonoBehaviour, IActivatable
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private NavMeshObstacle NavMeshObstacle;
    [SerializeField] private bool activateOnAwake = false;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioSource audioSource;

    private bool isOpen = false;

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
            audioSource.PlayOneShot(openClip);
            NavMeshObstacle.enabled = false;
            isOpen = true;
        }                           
    }
    public void Deactivate() 
    {
        if (isOpen) 
        {
            doorAnimator.SetTrigger("Close");
            audioSource.PlayOneShot(openClip);
            NavMeshObstacle.enabled = true;
            isOpen = false;
        }        
    }
    public void Toggle() 
    {
    
    }
}
