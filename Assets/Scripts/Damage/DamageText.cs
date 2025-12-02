using System;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public TMP_Text Text => text;

    [SerializeField] private Gradient gradient;

    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float yChangeBeforeDisable = 3f;

    private Transform cam;

    [Header("Anim")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip appearAnim;
    [SerializeField] private AnimationClip disappearAnim;


    [SerializeField] private float consecutiveWindow = 5f;
    float timer = 0f;

    float totalDmg = 0f;
    float displayedDmg = 0f;

    int animHash;
    

    private void Awake()
    {
        cam = Camera.main.transform;
        animHash = Animator.StringToHash(appearAnim.name);

    }
    private void OnEnable()
    {
        timer = 0f;
        totalDmg = 0f;
        displayedDmg = 0f;
        UpdateText();
    }

    private void Update()
    {
        transform.LookAt(cam, Vector3.up);
        transform.forward = cam.forward;

        timer += Time.deltaTime;
        if(timer >= consecutiveWindow) 
        {
            gameObject.SetActive(false);
        }
        
    }
    public Color GetDamageColor(float dmg) 
    {
        float t = Mathf.Clamp01(dmg / 100f);

        return gradient.Evaluate(t);
    }
    public void UpdateDmg(Vector3 pos, float dmg) 
    {        
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isPlayingThis =
            stateInfo.shortNameHash == animHash &&
            stateInfo.normalizedTime < 1f;

        if (isPlayingThis)
        {
            ApplyPendingDamage();
        }
        

        transform.position = pos;
        totalDmg += dmg;        
        animator.Play(appearAnim.name, -1, 0f);
        timer = 0f;        
    }
    public void ApplyPendingDamage() 
    {
        displayedDmg = totalDmg;
        UpdateText();
    }
    public void UpdateText() 
    {
        text.color = GetDamageColor(displayedDmg);
        text.text = Mathf.RoundToInt(displayedDmg).ToString();
    }
}
