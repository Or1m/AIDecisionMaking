using Characters.Actions;
using Characters.Enums;
using System.Collections.Generic;

namespace Characters.Skeleton
{
    public class SkeletonStateMachine : StateMachine
    {
        protected override Dictionary<ENPCAction, INPCAction> Mapper => new ()
        {
            [ENPCAction.Idle]       = new IdleAction(),
            [ENPCAction.Attacking]  = new AttackAction(),
            [ENPCAction.Seeking]    = new SeekAction(),
            [ENPCAction.Chasing]    = new ChaseAction(),
            [ENPCAction.Dead]       = new DeadAction(),
        };
    }
}
