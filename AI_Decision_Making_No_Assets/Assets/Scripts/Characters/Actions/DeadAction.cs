using Characters.Enums;
using UnityEngine;

namespace Characters.Actions
{
    public class DeadAction : INPCAction
    {
        public ENPCAction EnumValue => ENPCAction.Dead;

        public void OnStart(NPCBody body)
        {
            body.gameObject.SetActive(false);
        }
        public void OnUpdate(NPCBody body)
        {
            // Intentionally empty
        }
        public void OnTransit(NPCBody body)
        {
            Debug.LogError("Dead state should be the final one");
        }
    }
}
