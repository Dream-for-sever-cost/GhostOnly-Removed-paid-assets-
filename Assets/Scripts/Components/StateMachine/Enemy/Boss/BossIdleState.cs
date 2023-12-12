
public class BossIdleState : BossBaseState
{
    public BossIdleState(BossStateMachine sm) : base(sm) { }

    public override EBossState Key() => EBossState.Idle;

    public override EBossState NextState()
    {
        return base.NextState();
    }
}
