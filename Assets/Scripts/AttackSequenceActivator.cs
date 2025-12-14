using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AttackSequenceActivator : ActivatorBase, IActivatable
{
    [SerializeField] private List<EnemyAi> enemies = new();

    bool activated = false;
    public void Activate() 
    {
        activated = true;
    }
    public void Deactivate() 
    {
    
    }
    public void Toggle() 
    {
    
    }
    public void AddEnemy(EnemyAi ai) 
    {
        if (ai != null)
            enemies.Add(ai);
    }
    private void Update()
    {
        if (!activated || enemies == null || enemies.Count == 0) return;
        
        bool allDead = true;
        for (int i = 0; i < enemies.Count; i++) 
        {
            if(enemies[i] != null && !enemies[i].Damageable.Dead)
                allDead = false;
        }
        if (allDead) 
        {
            TriggerActivation();
            activated = false;
        }
    }
    
}
