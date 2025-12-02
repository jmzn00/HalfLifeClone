using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private Transform eyeLocation;

    [SerializeField] private float visionConeRange = 30f;
    public float VisionConeRange => visionConeRange;
    [SerializeField] private float visionConeAngle = 70f;
    public float VisionConeAngle => visionConeAngle;
    public Transform EyeLocation => eyeLocation;

    public float CosVisionConeAngle { get; private set; } = 0f;

    public virtual void Awake()
    {
        CosVisionConeAngle = Mathf.Cos(visionConeAngle * Mathf.Deg2Rad);
    }
    public virtual void ReportCanSee(DetectableTarget target) 
    {
    
    }
    public virtual void ReportLostSight(DetectableTarget target) 
    {
    
    }
}
