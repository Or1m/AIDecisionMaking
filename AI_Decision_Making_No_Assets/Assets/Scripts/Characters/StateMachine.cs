using Characters.Actions;
using Characters.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public abstract class StateMachine
    {
        private INPCAction currentAction;

        protected abstract Dictionary<ENPCAction, INPCAction> Mapper { get; }


        public bool ChangeState(ENPCAction newAction, NPCBody body)
        {
            if (!Mapper.TryGetValue(newAction, out var action))
            {
#if DEBUG
                Debug.LogError("This NPC archetype cannot do this action");
#endif
                return false;                
            }

            if (currentAction != null && newAction == currentAction.EnumValue)
                return false;

            currentAction?.OnTransit(body);
            currentAction = action;
            currentAction.OnStart(body);
            return true;
        }

        public void OnUpdate(NPCBody body)
        {
            currentAction.OnUpdate(body);
        }
    }
}
