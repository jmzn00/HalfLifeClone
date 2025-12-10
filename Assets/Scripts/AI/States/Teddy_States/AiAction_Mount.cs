using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI.States.Teddy_States
{
    [CreateAssetMenu(menuName = "Ai/Actions/Mount")]
    public class AiAction_Mount : AiAction
    {
        public override void OnEnter(EnemyAi controller)
        {
            controller.SetAnimTrigger("MountedIdle");
            StartMount(controller);
        }
        public override void Act(EnemyAi controller) 
        {
        
        }        
        private void StartMount(EnemyAi c) 
        {
            c.isMounted = true;
            c.CurrentTarget.LinkedAi.ToggleMounted(true);

            if (c.Agent.enabled) 
            {
                c.Agent.isStopped = true;
                c.Agent.enabled = false;
            }
            
            c.transform.SetParent(c.CurrentTarget.LinkedAi.MountLocation);
            c.transform.localPosition = Vector3.zero;
            c.transform.localRotation = Quaternion.identity;
        }
    }
}