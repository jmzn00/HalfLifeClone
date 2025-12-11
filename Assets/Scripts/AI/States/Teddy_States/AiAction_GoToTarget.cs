using UnityEngine;

namespace Assets.Scripts.AI.States.Teddy_States
{
    [CreateAssetMenu(menuName = "Ai/Actions/GoToTarget")]
    public class AiAction_GoToTarget : AiAction
    {

        public AiAudio audio;
        public bool lockTarget;
        public override void OnEnter(EnemyAi controller)
        {
            if(audio != null) 
            {
                controller.ClipTimer = 0f;
                controller.NextDelay = Random.Range(audio.delayRange.x, audio.delayRange.y);
            }            
            //controller.ClipTimer = 0f;
            //controller.NextDelay = Random.Range(delayRange.x, delayRange.y);
        }
        public override void Act(EnemyAi controller)
        {
            controller.SetAnimTrigger("Walk");

            if (controller.CurrentTarget) 
            {
                controller.Agent.SetDestination(controller.CurrentTarget.transform.position);
                controller.SetTargetLock(true);
            }
            
            if(audio)
                audio.UpdateAudio(controller);            
        }
    }
}