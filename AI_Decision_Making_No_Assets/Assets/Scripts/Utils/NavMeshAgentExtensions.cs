using UnityEngine.AI;

namespace Utils
{
    public static class NavMeshAgentExtensions
    {
        public static bool IsInDestination(this NavMeshAgent agent)
        {
            if (agent.pathPending)
                return false;

            if (agent.remainingDistance > agent.stoppingDistance)
                return false;

            if (agent.hasPath && agent.velocity.sqrMagnitude != 0f)
                return false;

            return true;
        }
    }
}
