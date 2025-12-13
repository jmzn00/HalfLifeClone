using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public struct Summonable 
{
    public GameObject prefab;
    public int amount;
}

namespace Assets.Scripts.AI.BossTeddy.Actions
{
    [CreateAssetMenu(menuName = "Ai/Actions/Summon")]
    public class AiAction_Summon : AiAction
    {        
        [Header("Summon Settings")]
        public List<Summonable> summonables;
        public float summonCooldown = 10f;
        public float windupTime = 1f;
        public float spawnRadius = 6f;
        public bool invulnerableDuringSummon = true;
        public LayerMask groundMask;

        private float _startTime;
        private bool _spawned;

        public override void OnEnter(EnemyAi controller)
        {
            base.OnEnter(controller);

            controller.SetAnimTrigger("Summon");
            controller.summonCooldownTimer = summonCooldown;
            controller.Agent.ResetPath();
            controller.SetInvulnerable(invulnerableDuringSummon);

            _startTime = Time.time;
            _spawned = false;
        }
        public override void OnExit(EnemyAi controller)
        {
            base.OnExit(controller);
        }
        public override void Act(EnemyAi controller)
        {
            if (_spawned) return;
            
            float elapsed = Time.time - _startTime;
            if(!_spawned && elapsed >= windupTime) 
            {
                SpawnWave(controller);
                _spawned = true;

                controller.SetInvulnerable(false);
                controller.attackCooldownTimer = 1f;
            }            
        }
        private void SpawnWave(EnemyAi controller) 
        {
            Vector3 center = controller.transform.position;

            foreach (var s in summonables) 
            {
                if (s.prefab == null || s.amount <= 0) continue;

                for (int i = 0; i < s.amount; i++) 
                {
                    Vector3 pos = GetRandomGroundPos(center, spawnRadius);
                    controller.Spawn(s.prefab, pos, Quaternion.identity);
                }
            }
        }
        private Vector3 GetRandomGroundPos(Vector3 center, float radius) 
        {
            Vector2 r = Random.insideUnitCircle.normalized * Random.Range(0.5f, radius);
            Vector3 p = center + new Vector3(r.x, 0f, r.y);

            if (Physics.Raycast(p + Vector3.up * 2f, Vector3.down, out var hit, 20f, groundMask, QueryTriggerInteraction.Ignore)) 
            {
                return hit.point;
            }
            return p;
        }
    }
}