using UnityEngine;

[RequireComponent(typeof(EnemyAi))]
[RequireComponent(typeof(DetectableTarget))]
public class AiMountableController : MonoBehaviour
{
    [SerializeField] private Collider[] colliders;

    [SerializeField] private string visionBlockingLayerName = "VisionBlocking";
    [SerializeField] private string mountedLayerName = "MountedNonBlocking";

    private int visionBlockingLayer;
    private int mountedLayer;

    private EnemyAi linkedAi;
    private DetectableTarget detectableTarget;
    private void Awake()
    {
        linkedAi = GetComponent<EnemyAi>();
        detectableTarget = GetComponent<DetectableTarget>();

        visionBlockingLayer = LayerMask.NameToLayer(visionBlockingLayerName);
        mountedLayer = LayerMask.NameToLayer(mountedLayerName);

        linkedAi.OnAiMountedChanged += ToggleMounted;
    }
    private void OnDisable()
    {
        linkedAi.OnAiMountedChanged -= ToggleMounted;
    }
    private void OnEnable()
    {
        ToggleMounted(false);
    }

    public void ToggleMounted(bool value) 
    {
        if (value) 
        {
            foreach (var c in colliders)
            {
                c.gameObject.layer = mountedLayer;
            }
            detectableTarget.enabled = false;
        }
        else 
        {
            foreach (var c in colliders)
            {
                c.gameObject.layer = visionBlockingLayer;
            }
            detectableTarget.enabled = true;
        }


    }
}
