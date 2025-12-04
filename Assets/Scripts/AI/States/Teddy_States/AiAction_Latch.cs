using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu(menuName = "Ai/Actions/Latch")]
public class AiAction_Latch : AiAction
{
    [Header("Latch Settings")]
    public float latchDuration = 3f;
    public Vector3 latchLocalPosition = new Vector3(0f, -1f,1f);
    public Quaternion latchLocalRotation = new Quaternion(0f, 180f, 0f, 1f);

    [Header("Grounding")]
    public LayerMask groundMask;
    public float dropRayDistance = 20f;

    [Header("Attack Settings")]
    public float damageAmount = 10f;
    public int attackAmount = 3;
    public float timeBetweenAttacks = 1f;

    private int attacksPerformed = 0;
    private float attackTimer = 0f;
    public override void Act(EnemyAi controller)
    {
        DetectableTarget target = controller.CurrentTarget;
        if (target == null)
            return;

        if (controller.transform.parent == null)
        {
            StartLatch(controller);
        }
        else
            UpdateLatch(controller);
    }
    public void StartLatch(EnemyAi c) 
    {
        c.transform.SetParent(Camera.main.transform);
        c.transform.localPosition = latchLocalPosition;
        c.transform.localRotation = latchLocalRotation;
        c.ToggleColliders(false);
        c.isAttacking = true;
        c.isLatched = true;
        c.attackCooldownTimer = latchDuration;

        attacksPerformed = 0;
        attackTimer = 0f;

        if(c.Agent != null)
            c.Agent.enabled = false;
    }
    public void UpdateLatch(EnemyAi c) 
    {
        if (c.attackCooldownTimer > 0f) 
        {
            while (attackTimer >= timeBetweenAttacks && attacksPerformed < attackAmount) 
            {
                GameServices.PlayerHealth.ApplyHit(new HitInfo()
                {
                    baseDamage = damageAmount,
                });
                attackTimer = 0f;
            }
            attackTimer += Time.deltaTime;
            return;
        }
        c.SetAnimTrigger("Latch");
        EndLatch(c);
    }
    private void EndLatch(EnemyAi c) 
    {
        c.transform.SetParent(null);
        c.ToggleColliders(true);

        if(c.Agent != null) 
        {
            c.Agent.enabled = true;

            if (NavMesh.SamplePosition(c.transform.position, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                c.Agent.Warp(navHit.position);
            }
            else
            {
                c.Agent.Warp(c.transform.position);
            }
        }
        c.isAttacking = false;
        c.isLatched = false;
    }

}
