using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HandAnimController : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController baseHandController;
    [SerializeField] private Animator handAnimator;

    public event Action OnReloadFinshed;

    public static readonly int DrawHash = Animator.StringToHash("Draw");
    public static readonly int AttackHash = Animator.StringToHash("Attack");
    public static readonly int ReloadHash = Animator.StringToHash("Reload");
    public void ReloadFinishedEvent() 
    {
        OnReloadFinshed?.Invoke();
    }
    public void PlayFire() 
    {
        handAnimator.Play(AttackHash, 0, 0f);
    }
    public void TriggerDraw()
    {
        handAnimator.ResetTrigger(ReloadHash);
        handAnimator.SetTrigger(DrawHash);    
    }
    public void TriggerReload() 
    {
        handAnimator.ResetTrigger(DrawHash);
        handAnimator.SetTrigger(ReloadHash);
    }
    public void ApplyOverride(HandAnimationSet set) 
    {
        if (set == null)
        {
            handAnimator.runtimeAnimatorController = baseHandController;
            Debug.LogError("No Hand Animation Set assigned");
            return;
        }
        handAnimator.runtimeAnimatorController = set.overrideController;
    }
    public bool IsAnimationPlaying(string stateName) 
    {
        var stateInfo = handAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime < 1f;
    }
}
