using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAi))]
public class VisionSensor : MonoBehaviour
{
    private EnemyAi linkedAi;
    private HashSet<DetectableTarget> currentlyVisibleTargets = new HashSet<DetectableTarget>();

    private void Awake()
    {
        linkedAi = GetComponent<EnemyAi>();
    }

    private void Update()
    {
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

            Vector3 targetPos = potentialTarget.transform.position + Vector3.up * 1.5f;
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

            if(Vector3.Dot(vectorToTarget.normalized, linkedAi.EyeLocation.forward) > linkedAi.CosVisionConeAngle) 
            {
                if(currentlyVisibleTargets.Contains(potentialTarget))
                    linkedAi.ReportLostSight(potentialTarget);
                continue;
            }
        }

    }
}
