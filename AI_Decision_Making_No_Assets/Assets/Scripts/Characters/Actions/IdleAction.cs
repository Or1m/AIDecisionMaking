using Characters.Enums;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Characters.Actions
{
    public class IdleAction : INPCAction
    {
        private const int walkRadius = 5;

        private const int minWaitTime = 3;
        private const int maxWaitTime = 10;

        private float waitTime = minWaitTime;

        public ENPCAction EnumValue => ENPCAction.Idle;


        public void OnStart(NPCBody body)
        {
            var agent = body.Agent;

            agent.isStopped = false;
            agent.SetDestination(GetRandomDestinationNearby(body));
        }
        public void OnUpdate(NPCBody body)
        {
            var agent = body.Agent;
            if (!agent.IsInDestination())
                return;

            if (waitTime > 0.0f)
            {
                waitTime -= Time.deltaTime;
                return;
            }

            waitTime = Random.Range(minWaitTime, maxWaitTime);
            agent.SetDestination(GetRandomDestinationNearby(body));
        }
        public void OnTransit(NPCBody body)
        {
            body.Agent.isStopped = true;
        }

        private Vector3 GetRandomDestinationNearby(NPCBody body)
        {
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += body.InitialPosition;
            
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkRadius, 1))
                finalPosition = hit.position;

            return finalPosition;
        }
    }
}
