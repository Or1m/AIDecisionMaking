using Characters.Enums;

namespace Characters.Actions
{
    public class ChaseAction : INPCAction
    {
        public ENPCAction EnumValue => ENPCAction.Chasing;


        public void OnStart(NPCBody body)
        {
            var agent = body.Agent;
            var target = body.EyeSight.Target;

            agent.isStopped = false;

            if (target)
                agent.SetDestination(target.transform.position);
        }
        public void OnUpdate(NPCBody body)
        {
            var agent = body.Agent;
            var target = body.EyeSight.Target;

            if (target)
                agent.SetDestination(target.transform.position);
        }
        public void OnTransit(NPCBody body)
        {
            body.Agent.isStopped = true;
        }
    }
}
