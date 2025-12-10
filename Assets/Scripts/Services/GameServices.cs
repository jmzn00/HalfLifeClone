using UnityEngine;
public struct Player 
{
    public PlayerHealth Health;
    public WeaponController Weapons;
    public DetectableTarget DetectableTarget;

}

[DefaultExecutionOrder(-100)]
public static class GameServices
{
    public static InputManager Input { get; internal set; }
    public static CameraManager Cam { get; internal set; }
    public static Pool Pool { get; internal set; }
    public static DetectableTargetManager DetectableTargetManager { get; internal set; }
    public static Player Player { get; internal set; }
    public static HitScript HitScript { get; internal set; }
    public static AudioManager AudioManager { get; internal set; }

}
