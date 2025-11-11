using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Camera playerCam;
    bool attackPending = false;
    private void Awake()
    {
        GameServices.Input.Actions.Player.Attack.performed += ctx => attackPending = true;
        GameServices.Input.Actions.Player.Attack.canceled += ctx => attackPending = false;

        playerCam = Camera.main;
    }

    private void Update()
    {
        if (attackPending)
        {
            Attack();
            attackPending = false;
        }
    }

    private void Attack() 
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if(Physics.Raycast(ray, out RaycastHit hit, 10f)) 
        {
            Hitbox hitbox = hit.collider.GetComponent<Hitbox>();
            if (hitbox) 
            {
                HitOutcome outcome = hitbox.ForwardHit(new HitInfo 
                {
                    point = hit.point,
                    normal = hit.normal,
                    isMelee = false,
                    baseDamage = 10f,
                    hitbox = hitbox.hitboxType
                });
            }
        }
    }
}
