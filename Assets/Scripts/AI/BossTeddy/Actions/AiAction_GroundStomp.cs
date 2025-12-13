using Mono.Cecil;
using NUnit.Framework.Constraints;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.AI.BossTeddy.Actions
{
    [CreateAssetMenu(menuName = "Ai/Actions/GroundStomp")]
    public class AiAction_GroundStomp : AiAction
    {
        [Header("Gameplay")]
        public float stompRadius = 5f;
        public float stompDamage = 15f;
        public LayerMask hitMask;
        public float knockbackForce = 8f;

        [Header("Timing")]
        public float windupTime = 0.6f;
        public float cooldownTime = 1.5f;

        [Header("Debug")]
        public bool drawGizmoz = true;

        private float _attackStartTime;
        private bool _damageDone;

        public override void OnEnter(EnemyAi controller)
        {
            base.OnEnter(controller);            
            controller.isAttacking = true;

            _attackStartTime = Time.time;
            _damageDone = false;

            controller.SetAnimTrigger("Attack");
        }
        public override void OnExit(EnemyAi controller)
        {
            base.OnExit(controller);
        }
        public override void Act(EnemyAi controller)
        {
            float elapsed = Time.time - _attackStartTime;

            if (! _damageDone && elapsed >= windupTime) 
            {
                DoStompDamage(controller);
                _damageDone = true;
            }
            if(elapsed >= windupTime) 
            {
                controller.isAttacking = false;
                controller.attackCooldownTimer = cooldownTime;
            }
        }
        private void DoStompDamage(EnemyAi controller) 
        {
            Vector3 center = controller.transform.position;

            Collider[] hits = Physics.OverlapSphere(
                center,
                stompRadius,
                hitMask,
                QueryTriggerInteraction.Ignore
                );

            foreach (var col in hits) 
            {
                var hitbox = col.GetComponent<Hitbox>();
                if (hitbox != null)
                    hitbox.ForwardHit(new HitInfo
                    {
                        baseDamage = stompDamage
                    });
            }
        }

#if UNITY_EDITOR
        public void DrawGizmos(EnemyAi controller)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controller.transform.position, stompRadius);
        }
#endif

    }
}