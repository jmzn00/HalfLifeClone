using System;
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
    [SerializeField] private Transform mountLocation;

    [SerializeField] private float visionConeRange = 30f;
    public float VisionConeRange => visionConeRange;
    [SerializeField] private float visionConeAngle = 70f;
    public float VisionConeAngle => visionConeAngle;
    public Transform EyeLocation => eyeLocation;
    public Transform MountLocation => mountLocation;

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
    public bool isMounted;

    // check if this ai has another ai mounted on it
    public bool hasMountedTarget;
    public NpcDamageable Damageable { get; private set; }
    private DetectableTarget detectableTarget;

    public event Action<bool> OnAiMountedChanged;
    public virtual bool CanAttack() 
    {
        return attackCooldownTimer <= 0f && !isAttacking;
    }
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
        detectableTarget = GetComponent<DetectableTarget>(); 
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
    public AiState currentState { get; private set; }
    public virtual void ChangeState(AiState newState) 
    {
        if(newState == null) 
        {
            Debug.LogWarning("State not Implemented");
            return;
        }
        if (currentState != null)
            currentState.OnExit(this);
        newState.OnEnter(this);
        currentState = newState;
        
    }
    public virtual void ToggleColliders(bool value) 
    {
        
    }    
    public virtual void ToggleMounted(bool value) 
    {   
        OnAiMountedChanged?.Invoke(value);
        hasMountedTarget = value;
    }
}
