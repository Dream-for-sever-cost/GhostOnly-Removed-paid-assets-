using UnityEngine;

namespace StateMachine.Slave
{
    public class SkullDeathState : SkullBaseState
    {

        public SkullDeathState(SkullStateMachine sm) : base(sm)
        {
        }

        public override ESkullState Key() => ESkullState.Death;

        public override void Enter()
        {
            base.Enter();
            StateMachine.SkullController.ReleaseEquip(StateMachine.transform.position);
            Managers.Target.RemoveSlave(StateMachine.transform);
        }
    }
}