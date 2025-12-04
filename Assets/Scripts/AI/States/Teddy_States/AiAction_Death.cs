using UnityEngine;

[CreateAssetMenu(menuName = "Ai/Actions/Death")]
public class AiAction_Death : AiAction
{
    public LayerMask groundLayer;
    Vector3 groundPos = new Vector3(999, 999, 999);
    bool startedDeath = false;
    public override void Act(EnemyAi c)
    {
        if (!c.Damageable.Dead) 
        {
            StartDeath(c);
        }
        else
            UpdateDeath(c);
        /*
        Vector3 groundPos = new Vector3(c.transform.position.x, 0f, c.transform.position.z);
        RaycastHit hit;
        if(Physics.Raycast(c.transform.position, Vector3.down, out hit, ~groundLayer)) 
        {
            groundPos = hit.point;
        }
        if(!c.Agent.isStopped)
            c.Agent.isStopped = true;
        c.transform.position = groundPos;
        c.ToggleColliders(false);
        c.SetAnimTrigger("Death");
        */
    }
    private void StartDeath(EnemyAi c) 
    {
        c.SetAnimTrigger("Death");
        if (c.Agent.enabled) 
        {
            c.Agent.isStopped = true;
            c.Agent.enabled = false;
        }
        
        c.ToggleColliders(false);
        c.Damageable.Dead = true;  
        groundPos = new Vector3(999,999,999);
    }
    private void UpdateDeath(EnemyAi c) 
    {
        if (groundPos == new Vector3(999,999,999)) 
        {
            RaycastHit hit;
            if(Physics.Raycast(c.transform.position, Vector3.down,out hit, ~groundLayer)) 
            {
                groundPos = hit.point;
            }
            else
                groundPos = new Vector3(c.transform.position.x, 0f, c.transform.position.z);
        }

        c.transform.position = Vector3.Lerp(c.transform.position, groundPos, Time.deltaTime * 5f);
    }
}
