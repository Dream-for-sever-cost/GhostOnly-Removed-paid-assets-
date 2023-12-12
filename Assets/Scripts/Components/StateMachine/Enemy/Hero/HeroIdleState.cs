using UnityEngine;

public class HeroIdleState : HeroBaseState
{
    public HeroIdleState(HeroStateMachine sm) : base(sm) { }

    public override EHeroState Key() => EHeroState.Idle;

    public override EHeroState NextState()
    {
        return base.NextState();
    }
}
