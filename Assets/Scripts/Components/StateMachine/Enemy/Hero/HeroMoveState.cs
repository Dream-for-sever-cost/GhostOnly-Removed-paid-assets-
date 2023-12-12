using UnityEngine;
using Util;

public class HeroMoveState : HeroBaseState
{
    private float recentMoveTime = Constants.Hero.SearchCycle; 

    public HeroMoveState(HeroStateMachine sm) : base(sm) { }

    public override EHeroState Key() => EHeroState.Move;

    public override EHeroState NextState()
    {
        return base.NextState();
    }

    public override void Enter()
    {
        StateMachine.Ani.SetBool(Constants.AniParams.Move, true);
    }

    public override void Exit()
    {
        StateMachine.Ani.SetBool(Constants.AniParams.Move, false);
    }

    public override void FixedUpdate()
    {
        if (StateMachine.IsMoving)
        {
            recentMoveTime -= Time.deltaTime;

            if (recentMoveTime < 0)
            {
                recentMoveTime = Constants.Hero.SearchCycle;
                StateMachine.Agent.Move(StateMachine.Target);
                StateMachine.FlipRenderer();
            }
        }
    }
}
