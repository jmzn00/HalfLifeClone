using Assets.Scripts.AI;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Ai/Actions/Wander Around Home")]
public class AiAction_WanderAroundHome : AiAction
{
    public float patrolRadius = 5f;
    public float moveTolerance = 0.2f;
    public float minWait = 1f;
    public float maxWait = 3f;
    public override void OnEnter(EnemyAi controller)
    {
        NavMeshAgent agent = controller.Agent;
        if (agent.hasPath && !agent.pathPending && agent.remainingDistance > moveTolerance)
        {
            agent.ResetPath();
        }
        if(AiAudio)
            AiAudio.OnEnter(controller);
    }
    public override void Act(EnemyAi controller)
    {
        NavMeshAgent agent = controller.Agent;        

        if (agent.remainingDistance <= moveTolerance) 
        {
            Vector2 rand = Random.insideUnitCircle * patrolRadius;
            Vector3 candidate = controller.transform.position + new Vector3(rand.x, 0f, rand.y);

            if(NavMesh.SamplePosition(candidate, out var hit, 1.5f, NavMesh.AllAreas)) 
            {
                agent.SetDestination(hit.position);
            }
        }
        controller.SetAnimTrigger("Walk");

        if(AiAudio)
            AiAudio.UpdateAudio(controller);
    }
}
