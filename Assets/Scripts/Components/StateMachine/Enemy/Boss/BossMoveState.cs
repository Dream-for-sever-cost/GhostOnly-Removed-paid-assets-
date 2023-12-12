using UnityEngine;
using Util;

public class BossMoveState : BossBaseState
{
    private float recentMoveTime = Constants.Hero.SearchCycle;

    public BossMoveState(BossStateMachine sm) : base(sm) { }

    public override EBossState Key() => EBossState.Move;

    public override EBossState NextState()
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