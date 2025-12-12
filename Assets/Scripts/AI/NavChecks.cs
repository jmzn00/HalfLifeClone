using UnityEngine;
using UnityEngine.AI;

public static class NavChecks
{
    public static bool HasValidPath(Vector3 from, Vector3 to, int areaMask = NavMesh.AllAreas) 
    {
        if (!NavMesh.SamplePosition(from, out var fromHit, 1.0f, areaMask)) return false;
        if (!NavMesh.SamplePosition(to, out var toHit, 1.0f, areaMask)) return false;

        var path = new NavMeshPath();
        bool ok = NavMesh.CalculatePath(fromHit.position, toHit.position, areaMask, path);
        return ok && path.status == NavMeshPathStatus.PathComplete;
    }
}
