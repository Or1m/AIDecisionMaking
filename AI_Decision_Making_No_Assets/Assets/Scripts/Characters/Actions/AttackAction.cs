using Characters.Enums;
using UnityEngine;

namespace Characters.Actions
{
    public class AttackAction : INPCAction
    {
        private float duration;

        public ENPCAction EnumValue => ENPCAction.Attacking;

        public void OnStart(NPCBody body)
        {
            // Intentionally empty
        }
        public void OnUpdate(NPCBody body)
        {
            duration += Time.deltaTime;
            if (duration >= body.AttackCooldown)
            {
                var target = body.EyeSight.Target;
                if (target == null)
                    return;
                
                if (!target.TryGetComponent<IDamageable>(out var damageable))
                    return;

                damageable.DealDamage(body.AttackDamage);
                duration = 0;
            }
        }
        public void OnTransit(NPCBody body)
        {
            body.Agent.isStopped = true;
        }
    }
}
