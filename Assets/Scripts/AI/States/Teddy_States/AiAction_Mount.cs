using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.AI.States.Teddy_States
{
    [CreateAssetMenu(menuName = "Ai/Actions/Mount")]
    public class AiAction_Mount : AiAction
    {
        public override void OnEnter(EnemyAi c)
        {
            c.AnimContoller.SetTrigger("Mount");
            StartMount(c);
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
            c.mountedAi = c.CurrentTarget.LinkedAi;
            c.transform.localPosition = Vector3.zero;
            c.transform.localRotation = Quaternion.identity;
        }
    }
}