using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.Duck.Actions
{
    [CreateAssetMenu(menuName = "Ai/Actions/Duck/Attack")]
    public class AiAction_Duck_Attack : AiAction
    {
        public float attackDamage = 15f;
        public float attackCooldown = 2f;        
        public override void OnEnter(EnemyAi controller)
        {
            base.OnEnter(controller);
        }
        public override void OnExit(EnemyAi controller)
        {
            base.OnExit(controller);
        }
        public override void Act(EnemyAi controller)
        {
            if(controller.attackCooldownTimer <= 0f) 
            {
                GameServices.Player.Health.ApplyHit(new HitInfo
                {
                    baseDamage = attackDamage
                });
                controller.attackCooldownTimer = attackCooldown;
                controller.isAttacking = true;
                controller.SetAnimTrigger("Attack");
            }
            else 
            {
                controller.isAttacking = false;                
            }
                
        }
    }
}