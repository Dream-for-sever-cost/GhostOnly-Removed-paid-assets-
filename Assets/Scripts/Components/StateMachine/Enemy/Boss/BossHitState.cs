using Util;

public class BossHitState : BossBaseState
{
    public BossHitState(BossStateMachine sm) : base(sm) { }

    public override EBossState Key() => EBossState.Hit;

    public override EBossState NextState()
    {
        return StateMachine.Health.IsDeath() ? EBossState.Death : EBossState.Idle;
    }

    public override void Enter()
    {
        StateMachine.Ani.SetTrigger(Constants.AniParams.Hit);
    }

    public override void Exit()
    {
        StateMachine.IsHit = false;
    }
}
