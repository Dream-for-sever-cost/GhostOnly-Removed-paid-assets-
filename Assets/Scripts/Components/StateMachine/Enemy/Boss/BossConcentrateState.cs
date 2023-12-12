using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class BossConcentrateState : BossBaseState
{
    private float currentConcentrateTime = 0;

    public BossConcentrateState(BossStateMachine sm) : base(sm) { }

    public override EBossState Key() => EBossState.Concentrate;

    public override EBossState NextState()
    {
        return (currentConcentrateTime < 0)? EBossState.Skill : Key();
    }

    public override void Enter()
    {
        currentConcentrateTime = Constants.Hero.BossConcentrateTime;
        StateMachine.Ani.SetBool(Constants.AniParams.Concentrate, true);
    }

    public override void Exit()
    {
        StateMachine.Ani.SetBool(Constants.AniParams.Concentrate, false);
    }

    public override void FixedUpdate()
    {
        currentConcentrateTime -= Time.deltaTime;
    }
}
