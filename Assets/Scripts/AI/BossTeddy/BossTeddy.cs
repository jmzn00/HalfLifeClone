using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BossTeddy : EnemyAi
{
    [SerializeField] private TMP_Text stateText;

    [Header("Minion Links")]
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private bool hideLinesWhenNoMinions = true;

    private readonly Dictionary<EnemyAi, LineRenderer> _lines = new();

    public override void Update()
    {
        base.Update();
        if (stateText)
            stateText.transform.forward = Camera.main.transform.forward;

        UpdateMinions();
    }
    private void UpdateMinions() 
    {
        if (minions != null)
            minions.RemoveAll(m => m == null || (m.Damageable != null && m.Damageable.Dead));


        bool hasMinions = minions != null && minions.Count > 0;
        SetInvulnerable(hasMinions);

        if (!hasMinions && hideLinesWhenNoMinions) 
        {
            foreach (var kv in _lines)             
                if (kv.Value) kv.Value.enabled = false;                            
            return;
        }

        if (hasMinions) 
        {
            foreach (var m in minions) 
            {
                if (m == null) continue;

                if (!_lines.TryGetValue(m, out var lr) || lr == null)
                {
                    if (!linePrefab) 
                    {
                        Debug.LogWarning($"{name}: linePrefab not set");
                        return;
                    }
                    lr = Instantiate(linePrefab, transform);
                    lr.positionCount = 2;
                    lr.useWorldSpace = true;
                    _lines[m] = lr;
                }

                Vector3 from = EyeLocation ? EyeLocation.position : transform.position;
                Vector3 to = m.EyeLocation ? m.EyeLocation.position : m.transform.position;

                lr.enabled = true;
                lr.SetPosition(0, from);
                lr.SetPosition(1, to);
            }
        }

        var toRemove = new List<EnemyAi>();

        foreach (var kv in _lines) 
        {
            var minion = kv.Key;
            if (minion == null || minions == null || !minions.Contains(minion) || minion.Damageable.Dead)
                toRemove.Add(minion);
        }
        foreach (var dead in toRemove) 
        {
            if (_lines.TryGetValue(dead, out var lr) && lr) 
            {
                Destroy(lr.gameObject);
            }
            _lines.Remove(dead);
        }
    }
    public override void ChangeState(AiState newState)
    {
        base.ChangeState(newState);

        if(stateText)
            stateText.text = newState.stateName;
    }
    [SerializeField] private Collider[] colliders;
    public override void ToggleColliders(bool value)
    {
        foreach(Collider collider in colliders) 
        {
            if(collider != null)
                collider.enabled = value;
        }
    }
    public override void AddMinion(EnemyAi ai)
    {
        base.AddMinion(ai);
    }
}
