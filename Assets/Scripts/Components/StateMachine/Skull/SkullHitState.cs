using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace StateMachine.Slave
{
    public class SkullHitState : SkullBaseState
    {

        public SkullHitState(SkullStateMachine sm) : base(sm)
        {
        }

        public override ESkullState Key() => ESkullState.Hit;

        public override ESkullState NextState()
        {
            return IsDeath ? ESkullState.Death : ESkullState.Idle;
        }
        public override void Enter()
        {
            base.Enter();
            StateMachine.IsHit = false;
            if (StateMachine.Target == null && !StateMachine.isLamper)
            {
                StateMachine.followRange = Constants.Distance.ChaseDistance;
            }
        }
    }
}