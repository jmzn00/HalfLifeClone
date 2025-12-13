using UnityEngine;

namespace Assets.Scripts.AI.States.Teddy_States
{
    [CreateAssetMenu(menuName = "Ai/Actions/GoToTarget")]
    public class AiAction_GoToTarget : AiAction
    {
        public AiAudio audio;

        public float minDistance = 1f;
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
                float dist = Vector3.Distance(controller.transform.position, controller.CurrentTarget.transform.position);
                if (dist > minDistance) 
                {
                    controller.Agent.SetDestination(controller.CurrentTarget.transform.position);
                }
                else 
                {
                    controller.Agent.ResetPath();
                }
                
            }
            
            if(audio)
                audio.UpdateAudio(controller);            
        }
    }
}