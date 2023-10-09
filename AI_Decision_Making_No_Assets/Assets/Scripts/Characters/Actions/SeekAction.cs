using Characters.Enums;
using UnityEngine;

namespace Characters.Actions
{
    public class SeekAction : INPCAction
    {
        private Vector3 lastTargetPos;

        public ENPCAction EnumValue => ENPCAction.Seeking;

        public void OnStart(NPCBody body)
        {
            var agent = body.Agent;

            agent.isStopped = false;
            lastTargetPos = body.Hearing.Target.Position;
            agent.SetDestination(lastTargetPos);
        }
        public void OnUpdate(NPCBody body)
        {
            var newPos = body.Hearing.Target.Position;

            if (lastTargetPos != newPos)
                body.Agent.SetDestination(newPos);
        }
        public void OnTransit(NPCBody body)
        {
            body.Agent.isStopped = true;
        }
    }
}
