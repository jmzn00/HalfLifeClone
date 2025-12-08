using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu(menuName = "Ai/Actions/Latch")]
public class AiAction_Latch : AiAction
{
    [Header("Latch Settings")]
    public float latchDuration = 3f;
    public Vector3 latchLocalPosition = new Vector3(0f, -1f,1f);
    public Quaternion latchLocalRotation = new Quaternion(0f, 180f, 0f, 1f);
    public float cooldownAfterLatch = 2f;

    [Header("Grounding")]
    public LayerMask groundMask;
    public float dropRayDistance = 20f;

    [Header("Attack Settings")]
    public float damageAmount = 10f;
    public int attackAmount = 3;

    private float timeBetweenAttacks;
    private int attacksPerformed = 0;
    private float attackTimer = 0f;

    

    private float latchTimer = 0f;

    public override void Act(EnemyAi controller)
    {        
        if (controller.transform.parent == null && !controller.isLatched)
        {
            StartLatch(controller);
        }
        else
        {
            UpdateLatch(controller);
        }
    }

    private void StartLatch(EnemyAi c)
    {
        c.isLatched = true;
        c.isAttacking = true;

        c.transform.SetParent(Camera.main.transform);
        c.transform.localPosition = latchLocalPosition;
        c.transform.localRotation = latchLocalRotation;
        c.ToggleColliders(false);
        
        

        timeBetweenAttacks = latchDuration / attackAmount;
        latchTimer = latchDuration;
        attacksPerformed = 0;
        attackTimer = 0f;

        c.SetAnimTrigger("Latch");

        if (c.Agent != null)
            c.Agent.enabled = false;
    }

    private void UpdateLatch(EnemyAi c)
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttacks && attacksPerformed < attackAmount)
        {
            GameServices.Player.Health.ApplyHit(new HitInfo 
            {
                baseDamage = damageAmount
            });
            attacksPerformed++;
            attackTimer = 0f;
        }
      
        latchTimer -= Time.deltaTime;
        if (latchTimer <= 0f)
        {
            EndLatch(c);
        }
    }

    private void EndLatch(EnemyAi c)
    {
        c.transform.SetParent(null);
        c.ToggleColliders(true);

        if (c.Agent != null)
        {
            c.Agent.enabled = true;

            if (NavMesh.SamplePosition(c.transform.position, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
                c.Agent.Warp(navHit.position);
            else
                c.Agent.Warp(c.transform.position);
        }

        c.attackCooldownTimer = cooldownAfterLatch;

        c.isAttacking = false;
        c.isLatched = false;
    }

}
