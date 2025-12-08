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

    [Header("Misc")]
    public LayerMask groundLayer;    

    public override void Act(EnemyAi controller)
    {
        if (controller.isAttacking) 
        {
            UpdateLeap(controller);
            return;
        }

        DetectableTarget target = controller.CurrentTarget;
        if (target == null) return;

        Vector3 startPos = controller.transform.position;
        Vector3 targetPos = target.transform.position + target.transform.forward;

        // Horizontal distance for range / direction
        Vector3 toTargetXZ = target.transform.position - startPos;
        toTargetXZ.y = 0f;
        float horizDistance = toTargetXZ.magnitude;

        if (!controller.isAttacking)
        {
            // Move toward player until in leap range
            if (horizDistance > attackRange)
            {
                controller.SetAnimTrigger("Walk");
                if (controller.Agent != null)
                    controller.Agent.SetDestination(target.transform.position);
                return;
            }

            if (controller.attackCooldownTimer > 0f)
                return;

            StartLeap(controller, startPos, targetPos, toTargetXZ, horizDistance);
        }
        else
        {
            UpdateLeap(controller);
        }
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
    }

    private void UpdateLeap(EnemyAi c)
    {
        c.SetAnimTrigger("Latch");
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
                if(NavMesh.SamplePosition(c.transform.position, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas)) 
                {
                    c.transform.position = navHit.position;
                    c.Agent.enabled = true;
                }
            }
        }
    }

}
