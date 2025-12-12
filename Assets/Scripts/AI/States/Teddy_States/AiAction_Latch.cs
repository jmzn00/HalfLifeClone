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
    public float timeBetweenAttacks = 0.5f;

    private float latchTimer;
    private float attackTimer = 0f;
    private int attacksPerformed = 0;

    public AudioClip latchSound;

    public override void OnEnter(EnemyAi controller)
    {
        StartLatch(controller);
        controller.isLatched = true;

        var request = new AudioRequest
            (
                latchSound,
                5,
                controller.transform.position,
                1f
            );
        GameServices.AudioManager.SendRequest(request);
    }
    public override void Act(EnemyAi controller)
    {        
        UpdateLatch(controller);
    }
    
    public override void OnExit(EnemyAi controller)
    {
        base.OnExit(controller);        
    }

    private void StartLatch(EnemyAi c)
    {
        c.ToggleColliders(false);
        c.transform.SetParent(Camera.main.transform);
        c.transform.localPosition = latchLocalPosition;
        c.transform.localRotation = latchLocalRotation;

        if(c.Agent != null)
            c.Agent.enabled = false;
        c.SetAnimTrigger("Attack");
        attackTimer = timeBetweenAttacks + 1f;
        latchTimer = latchDuration;
        attacksPerformed = 0;

        c.isAttacking = true;
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
    }

}
