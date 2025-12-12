using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Ai/Actions/Leap Attack")]
public class AiAction_LeapAttack : AiAction
{
    [Header("Leap Settings")]
    public float attackRange = 4f;
    public float apexHeight = 2f;
    public float maxHorizontalSpeed = 10f;
    public float cooldown = 2f;
    public bool lockTarget = false;
    
    [Header("Damage")]
    public float attackDamage = 10f;
    public float attackDistance = 1f;

    [Header("Target Settings")]
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.75f, 0f);
    [SerializeField] private Vector3 enemyOffset = new Vector3(0f, 1f, 0f);

    [Header("Misc")]
    public LayerMask groundLayer;

    public override void OnEnter(EnemyAi controller)
    {
        base.OnEnter(controller);

        controller.SetTargetLock(lockTarget);
        DetectableTarget target = controller.CurrentTarget;

        Vector3 startPos = controller.transform.position;
        Vector3 targetPos = target.transform.position + target.transform.forward;

        // Horizontal distance for range / direction
        Vector3 toTargetXZ = target.transform.position - startPos;
        toTargetXZ.y = 0f;
        float horizDistance = toTargetXZ.magnitude;

        StartLeap(controller, startPos, targetPos, toTargetXZ, horizDistance);
    }
    public override void OnExit(EnemyAi c)
    {
        base.OnExit(c);
    }

    public override void Act(EnemyAi controller)
    {
        UpdateLeap(controller);
    }

    private void StartLeap(EnemyAi c, Vector3 startPos, Vector3 targetPos, Vector3 toTargetXZ, float horizDistance)
    {
        if (c.Agent != null)
            c.Agent.enabled = false;

        float gravity = Mathf.Abs(Physics.gravity.y);

        float maxY = Mathf.Max(startPos.y, targetPos.y) + apexHeight;
        float vY = Mathf.Sqrt(2f * gravity * (maxY - startPos.y));

        float timeUp = vY / gravity;
        float deltaYDown = maxY - targetPos.y;
        float timeDown = Mathf.Sqrt(2f * deltaYDown / gravity);
        float totalTime = timeUp + timeDown;

        float horizSpeed = horizDistance / Mathf.Max(totalTime, 0.01f);
        horizSpeed = Mathf.Min(horizSpeed, maxHorizontalSpeed);

        Vector3 dirXZ = horizDistance > 0.01f ? toTargetXZ.normalized : Vector3.zero;
        Vector3 vXZ = dirXZ * horizSpeed;

        c.attackVelocity = vXZ + Vector3.up * vY;
        c.isAttacking = true;
        c.attackCooldownTimer = cooldown;
        c.attackAirTime = 0f;

        if (dirXZ.sqrMagnitude > 0.0001f)
            c.transform.forward = dirXZ;

        c.leapHasHit = false;
    }

    private void UpdateLeap(EnemyAi c)
    {
        c.SetAnimTrigger("Attack");
        // Track time in air
        c.attackAirTime += Time.deltaTime;

        // Apply gravity
        c.attackVelocity += Physics.gravity * Time.deltaTime;
        Vector3 displacement = c.attackVelocity * Time.deltaTime;

        // Move (we're ignoring Rigidbody for now so we know this always applies)
        c.transform.position += displacement;

        // Face movement direction
        Vector3 flatVel = new Vector3(c.attackVelocity.x, 0f, c.attackVelocity.z);
        if (flatVel.sqrMagnitude > 0.001f)
            c.transform.forward = flatVel.normalized;

        Vector3 enemyPos = c.transform.position + enemyOffset;
        Vector3 targetPos = c.CurrentTarget.transform.position + targetOffset;
        float distanceToTarget = Vector3.Distance(enemyPos, targetPos);
        if(!c.leapHasHit && distanceToTarget <= attackDistance)
        {
            c.leapHasHit = true;
            GameServices.Player.Health.ApplyHit(new HitInfo
            {
                baseDamage = attackDamage
            });
        }

        // -------- LANDING LOGIC --------

        // Only start checking for ground after we've been in air a bit,
        // and only when we're moving downward (so it doesn't instantly cancel)
        const float minAirTimeBeforeLandCheck = 0.1f;
        const float raycastDistance = 1.5f;
        const float maxLeapTime = 1.5f; // safety net

        bool shouldLand = false;

        if (c.attackAirTime > minAirTimeBeforeLandCheck && c.attackVelocity.y <= 0f)
        {
            // Raycast from slightly above the pivot, downward
            Vector3 rayOrigin = c.transform.position + Vector3.up * 0.5f;
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
            {
                // Snap to ground so we don't sink/hover
                c.transform.position = hit.point;
                shouldLand = true;
            }
        }

        // Safety: if something goes wrong with raycast, force landing after maxLeapTime
        if (!shouldLand && c.attackAirTime >= maxLeapTime)
        {
            shouldLand = true;
        }

        if (shouldLand)
        {
            c.isAttacking = false;
            c.attackVelocity = Vector3.zero;
            c.attackAirTime = 0f;

            if(c.Agent != null) 
            {
                c.Agent.enabled = true;

                if(NavMesh.SamplePosition(c.transform.position, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas)) 
                {
                    c.Agent.Warp(navHit.position);
                }
                else
                {
                    c.Agent.Warp(c.transform.position);
                }
            }
            Debug.Log("LeapFinished");
            return;
        }
    }

}
