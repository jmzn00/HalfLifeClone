using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAi))]
public class VisionSensor : MonoBehaviour
{
    private EnemyAi linkedAi;
    private HashSet<DetectableTarget> currentlyVisibleTargets = new HashSet<DetectableTarget>();
    Vector3 lastHitPoint;
    bool lastHitWasTarget;

    [SerializeField] private LayerMask DetectionMask;

    private void Awake()
    {
        linkedAi = GetComponent<EnemyAi>();
    }

    private void Update()
    {
        if (linkedAi.Damageable.Dead || !linkedAi.Activated) return;

        var visibleThisFrame = new HashSet<DetectableTarget>();
        DetectableTarget closestTarget = null;
        float closestDistanceSqr = float.MaxValue;
        Vector3 closestHitPoint = Vector3.zero;

        List<DetectableTarget> detectableTargets = GameServices.DetectableTargetManager.DetectableTargets;

        for(int i = 0; i < detectableTargets.Count; i++) 
        {
            DetectableTarget potentialTarget = detectableTargets[i];
            if (potentialTarget == gameObject) 
            {                
                continue;
            }

            Vector3 targetPos = potentialTarget.transform.position + Vector3.up;
            Vector3 vectorToTarget = targetPos - linkedAi.EyeLocation.position;
            float sqrDistance = vectorToTarget.sqrMagnitude;

            if(sqrDistance > (linkedAi.VisionConeRange * linkedAi.VisionConeRange)) 
            {
                if (currentlyVisibleTargets.Contains(potentialTarget)) 
                {
                    linkedAi.ReportLostSight(potentialTarget);                  
                }
                continue;
            }

            if(Vector3.Dot(vectorToTarget.normalized, linkedAi.EyeLocation.forward) < linkedAi.CosVisionConeAngle) 
            {
                if(currentlyVisibleTargets.Contains(potentialTarget))
                    linkedAi.ReportLostSight(potentialTarget);
                continue;
            }
            RaycastHit hitInfo;
            if(Physics.Raycast(linkedAi.EyeLocation.position, vectorToTarget.normalized, out hitInfo, linkedAi.VisionConeRange, DetectionMask, QueryTriggerInteraction.Ignore)) 
            {
                DetectableTarget hitTarget = hitInfo.collider.GetComponentInParent<DetectableTarget>();
                bool canSee = hitTarget == potentialTarget;

                if (canSee)
                {
                    visibleThisFrame.Add(potentialTarget);

                    if(sqrDistance < closestDistanceSqr) 
                    {
                        closestDistanceSqr = sqrDistance;
                        closestTarget = potentialTarget;
                        closestHitPoint = hitInfo.point;
                    }
                    else 
                    {
                        if (currentlyVisibleTargets.Contains(potentialTarget))
                            linkedAi.ReportLostSight(potentialTarget);
                    }
                }
                else 
                {
                    if (currentlyVisibleTargets.Contains(potentialTarget))
                        linkedAi.ReportLostSight(potentialTarget);
                }
            }
            if(closestTarget != null) 
            {
                linkedAi.ReportCanSee(closestTarget);
                lastHitPoint = closestHitPoint;
                lastHitWasTarget = true;                                
            }
            else 
            {
                lastHitWasTarget = false;
            }
            foreach (var prevTarget in currentlyVisibleTargets)
            {
                if (!visibleThisFrame.Contains(prevTarget))
                    linkedAi.ReportLostSight(prevTarget);
            }
            currentlyVisibleTargets = visibleThisFrame;
        }

    }
    void OnDrawGizmos()
    {
        if (linkedAi == null || !linkedAi.enabled) return;

        Gizmos.color = lastHitWasTarget ? Color.green : Color.red;
        Gizmos.DrawLine(linkedAi.EyeLocation.position, lastHitPoint);
        Gizmos.DrawWireSphere(lastHitPoint, 0.1f);
    }
}
