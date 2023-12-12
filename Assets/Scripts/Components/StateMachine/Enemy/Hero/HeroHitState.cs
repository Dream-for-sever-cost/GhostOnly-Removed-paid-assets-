using UnityEngine;
using Util;

public class HeroHitState : HeroBaseState
{
    public HeroHitState(HeroStateMachine sm) : base(sm) { }

    public override EHeroState Key() => EHeroState.Hit;

    public override EHeroState NextState()
    {
        return StateMachine.Health.IsDeath() ? EHeroState.Death : EHeroState.Idle;
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
