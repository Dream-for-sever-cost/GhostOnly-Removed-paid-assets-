using UnityEngine;
using Util;

public class HeroAttackState : HeroBaseState
{
    public HeroAttackState(HeroStateMachine sm) : base(sm) { }

    public override EHeroState Key() => EHeroState.Attack;

    public override EHeroState NextState()
    {
        return EHeroState.Idle;
    }

    public override void Enter()
    {
        StateMachine.Ani.SetTrigger(Constants.AniParams.Action);
        StateMachine.Agent.Stop();
        Attack(StateMachine.Target);

        StateMachine.IsAttack = false;
    }

    private void Attack(Transform target)
    {
        if (target == null)
            return;

        Vector2 dir = (target.position - StateMachine.transform.position).normalized;
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        StateMachine.RotateEquip(rotZ);
        StateMachine.Attack.Action(dir, StateMachine.AttackSpawnPoint.position, rotZ - 90);
    }
}
