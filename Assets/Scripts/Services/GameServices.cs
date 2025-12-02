using UnityEngine;

[DefaultExecutionOrder(-100)]
public static class GameServices
{
    public static WeaponController WeaponController { get; internal set; }
    public static InputManager Input { get; internal set; }
    public static CameraManager Cam { get; internal set; }
    public static Pool Pool { get; internal set; }
    public static DetectableTargetManager DetectableTargetManager { get; internal set; }

    public static GameObject PlayerObject { get; internal set; }

}
