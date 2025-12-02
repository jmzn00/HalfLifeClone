using UnityEngine;

[CreateAssetMenu(menuName = "Ai/Transitions/LeapToLatch")]
public class AiTransition_LeapToLatch : AiTransition
{
    public float latchDistance = 0.5f;
    public Vector3 playerOffset = new Vector3(0f, 1f, 0f);

    public AiState latchState;
    public override AiState Check(EnemyAi controller)
    {
        if (!controller.isAttacking) return null;

        DetectableTarget target = controller.CurrentTarget;
        if (target == null) return null;

        Vector3 enemyPos = controller.transform.position;
        Vector3 targetPos = target.transform.position + playerOffset;

        float distance = Vector3.Distance(enemyPos, targetPos);

        if(distance <= latchDistance) 
        {
            return latchState;
        }
        return null;
    }
}
