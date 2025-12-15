using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 3f;
    private PlayerInventory inventory;
    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }
    private void OnEnable()
    {

        GameServices.Input.Actions.Player.Interact.performed += ctx => Interact();        
    }
    private void Interact() 
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance)) 
        {
            if (hit.collider.CompareTag("Interactable")) 
            {
                if(inventory.Equipped != null) 
                {
                    if(inventory.Equipped.TryUse(new UseContext
                    {
                        user = gameObject,
                        target = hit.collider.gameObject
                    })) 
                    {
                        inventory.TryRemoveItem(inventory.Equipped);
                    }

                    
                }
            }
        }
    }
}
