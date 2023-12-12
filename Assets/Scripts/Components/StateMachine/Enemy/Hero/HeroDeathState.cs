using UnityEngine;
using Util;

public class HeroDeathState : HeroBaseState
{
    private float currentDeathDuration = 0f;

    public HeroDeathState(HeroStateMachine sm) : base(sm) { }

    public override EHeroState Key() => EHeroState.Death;

    public override EHeroState NextState()
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
            Managers.Soul.GetSoul(Constants.Hero.HeroReward);
            StateMachine.Agent.ToggleEnable(true);

            StateMachine.Pool.ReleaseObject();
        }
    }
}