using UnityEngine;
using Util;

public class BossDeathState : BossBaseState
{
    private float currentDeathDuration = 0f;

    public BossDeathState(BossStateMachine sm) : base(sm) { }

    public override EBossState Key() => EBossState.Death;

    public override EBossState NextState()
    {
        return Key();
    }

    public override void Enter()
    {
        StateMachine.Ani.SetBool(Constants.AniParams.Die, true);
        StateMachine.Agent.Stop();

        StateMachine.ChangeOnDieUI(false);
        StateMachine.Agent.ToggleEnable(false);

        currentDeathDuration = Constants.Hero.DieDuration;

        Managers.Target.RemoveEnemy(StateMachine.transform);
    }

    public override void FixedUpdate()
    {
        currentDeathDuration -= Time.deltaTime;

        if (currentDeathDuration < 0)
        {
            Managers.GameManager.heroDeathCount += 1;
            StateMachine.Ani.SetBool(Constants.AniParams.Die, false);
            Managers.Soul.GetSoul(Constants.Hero.BossReward);
            StateMachine.Agent.ToggleEnable(true);

            StateMachine.Pool.ReleaseObject();
        }
    }
}