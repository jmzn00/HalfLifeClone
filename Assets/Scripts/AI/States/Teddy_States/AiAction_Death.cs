using UnityEngine;

[CreateAssetMenu(menuName = "Ai/Actions/Death")]
public class AiAction_Death : AiAction
{
    public LayerMask groundLayer;
    Vector3 groundPos;
    public override void Act(EnemyAi c)
    {
        if (!c.Damageable.Dead) 
        {
            StartDeath(c);
        }
        else
            UpdateDeath(c);
    }
    private void StartDeath(EnemyAi c) 
    {       
        if (c.Agent.enabled) 
        {
            c.Agent.isStopped = true;
            c.Agent.enabled = false;
        }
        c.SetAnimTrigger("Death");
        c.ToggleColliders(false);
        c.Damageable.Dead = true;

        Debug.Log("Enemy Dead");
    }
    private void UpdateDeath(EnemyAi c) 
    {        
        RaycastHit hit;
        if (Physics.Raycast(
        c.transform.position,
        Vector3.down,
        out hit,
        Mathf.Infinity,
        ~groundLayer
    ))
        {
            groundPos = hit.point;
        }
        else
        {
            groundPos = new Vector3(c.transform.position.x, 0f, c.transform.position.z);
        }

        c.transform.position = Vector3.Lerp(c.transform.position, groundPos, Time.deltaTime * 5f);
    }
}
