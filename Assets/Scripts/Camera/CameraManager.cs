using UnityEngine;

[DefaultExecutionOrder(-99)]
public class CameraManager : MonoBehaviour
{
    private Transform followTarget = null;
    private Camera cam;

    [SerializeField] private Vector3 playerCamOffset;

    private void Awake()
    {
        if(GameServices.Cam != this)
            GameServices.Cam = this;

        cam = Camera.main;
    }
    private void OnDisable()
    {
        if(GameServices.Cam == this)
            GameServices.Cam = null;
    }

    MovementController movementController = null;
    public void SetFpsCamera(MovementController mc) 
    {
        movementController = mc;
    }
    private void Update()
    {
        if (!movementController) return;

        cam.transform.position = movementController.transform.position + playerCamOffset;
        cam.transform.rotation = Quaternion.Euler(movementController.Pitch, movementController.Yaw, 0f);
    }
}
