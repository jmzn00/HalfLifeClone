using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AiAnimationContoller))]
[RequireComponent(typeof(NpcDamageable))]
//[RequireComponent(typeof(Rigidbody))]
public class EnemyAi : MonoBehaviour, IActivatable
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
    public float summonCooldownTimer;

    public bool isLatched;
    public bool isMounted;

    // check if this ai has another ai mounted on it
    public bool hasMountedTarget;
    public NpcDamageable Damageable { get; private set; }
    private DetectableTarget detectableTarget;

    public event Action<bool> OnAiMountedChanged;

    private bool lockTarget = false;
    [SerializeField] private AiState defualtState;

    public bool Activated { get; private set; }

    [HideInInspector] public float ClipTimer;
    [HideInInspector] public float NextDelay;
    [HideInInspector] public bool leapHasHit;


    [SerializeField] private bool activateOnAwake = false;

    [SerializeField] private EnemyAi mountableAi;

    public void Activate() 
    {
        Activated = true;
    }
    public void Deactivate() 
    {
        Activated = false;
    }
    public void Toggle() 
    {
        
    }    
    public virtual void SetTargetLock(bool value) 
    {
        lockTarget = value;
    }
    public virtual void SetTarget(DetectableTarget target) 
    {
        CurrentTarget = target;
    }
    public virtual bool CanAttack() 
    {
        return attackCooldownTimer <= 0f && !isAttacking;
    }
    public virtual GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot) 
    {
        GameObject go = Instantiate(prefab, pos, rot);
        EnemyAi minion = go.GetComponent<EnemyAi>();
        if(minion)
            AddMinion(minion);
        return go;
    }
    public EnemyAi mountedAi = null;
    public void SetInvulnerable(bool value) 
    {
        Damageable.SetInveulnerable(value);
    }
    public List<EnemyAi> minions = new();
    public virtual void AddMinion(EnemyAi ai) 
    {
        if(ai != null)
            minions.Add(ai);
    }
    public virtual void Update()
    {        
        if(currentState != null && Activated) 
        {
            currentState.UpdateState(this);
        }
        
        CommonUpdate();
    }
    protected virtual void CommonUpdate() 
    {
        if(summonCooldownTimer > 0f) 
        {
            summonCooldownTimer -= Time.deltaTime;
        }
        if(attackCooldownTimer > 0f) 
        {
            attackCooldownTimer -= Time.deltaTime;
            isAttacking = true;
        }
        else 
        {
            isAttacking = false;
        }
    }
    public virtual void Awake()
    {
        CosVisionConeAngle = Mathf.Cos(visionConeAngle * Mathf.Deg2Rad);

        Agent = GetComponent<NavMeshAgent>();
        AnimContoller = GetComponent<AiAnimationContoller>();
        Damageable = GetComponent<NpcDamageable>();
        detectableTarget = GetComponent<DetectableTarget>(); 

        ChangeState(defualtState);

        if (activateOnAwake)
            Activate();

        GameServices.AiManager.Register(this);

        if (mountableAi)
            mountedAi = mountableAi;
    }
    private void OnEnable()
    {
        GameServices.AiManager?.Register(this);
    }
    private void OnDisable()
    {
        GameServices.AiManager?.Unregister(this);
    }
    public virtual void SetAnimTrigger(string t) 
    {
        AnimContoller.SetTrigger(t);
    }
    public virtual void ReportCanSee(DetectableTarget target) 
    {
        if (lockTarget)
            return;

        CurrentTarget = target;
    }
    public virtual void ReportLostSight(DetectableTarget target) 
    {
        if (lockTarget && CurrentTarget)
            return;

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
