using UnityEngine;
public enum DetectableType
{
   Player,
   AiEnemy,
   AiMountable
}
[DefaultExecutionOrder(-98)]
public class DetectableTarget : MonoBehaviour
{
    [SerializeField] private DetectableType type;
    public DetectableType Type => type;
    private EnemyAi linkedAi;
    public EnemyAi LinkedAi => linkedAi;    
    private void OnEnable()
    {
        GameServices.DetectableTargetManager.RegisterDetectable(this);
    }
    private void OnDisable()
    {
        GameServices.DetectableTargetManager.UnregisterDetectable(this);
    }
    private void Awake()
    {
        if(type == DetectableType.AiMountable || type == DetectableType.AiEnemy)
            linkedAi = GetComponent<EnemyAi>();
    }
}
