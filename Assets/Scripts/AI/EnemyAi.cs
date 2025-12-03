using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AiAnimationContoller))]
[RequireComponent(typeof(NpcDamageable))]
//[RequireComponent(typeof(Rigidbody))]
public class EnemyAi : MonoBehaviour
{
    [Header("Prefs")]
    [SerializeField] private Transform eyeLocation;

    [SerializeField] private float visionConeRange = 30f;
    public float VisionConeRange => visionConeRange;
    [SerializeField] private float visionConeAngle = 70f;
    public float VisionConeAngle => visionConeAngle;
    public Transform EyeLocation => eyeLocation;

    public float CosVisionConeAngle { get; private set; } = 0f;

    public NavMeshAgent Agent { get; private set; }
    //public Rigidbody Rb { get; private set; }
    public AiAnimationContoller AnimContoller { get; private set; }
    public DetectableTarget CurrentTarget { get; private set; }

    public bool isAttacking;
    public Vector3 attackVelocity;
    public float attackCooldownTimer;
    public float attackAirTime;

    public bool isLatched; 
    public NpcDamageable Damageable { get; private set; }    
    protected virtual void CommonUpdate() 
    {
        if(attackCooldownTimer > 0f)
            attackCooldownTimer -= Time.deltaTime;
    }
    public virtual void Awake()
    {
        CosVisionConeAngle = Mathf.Cos(visionConeAngle * Mathf.Deg2Rad);
        Agent = GetComponent<NavMeshAgent>();
        AnimContoller = GetComponent<AiAnimationContoller>();
        Damageable = GetComponent<NpcDamageable>();
        //Rb = GetComponent<Rigidbody>();
    }
    public virtual void SetAnimTrigger(string t) 
    {
        AnimContoller.SetTrigger(t);
    }
    public virtual void ReportCanSee(DetectableTarget target) 
    {
        CurrentTarget = target;
    }
    public virtual void ReportLostSight(DetectableTarget target) 
    {
        if(CurrentTarget == target)
            CurrentTarget = null;
    }
    public virtual void ChangeState(AiState state) 
    {
    
    }
    public virtual void ToggleColliders(bool value) 
    {
    
    }
}
