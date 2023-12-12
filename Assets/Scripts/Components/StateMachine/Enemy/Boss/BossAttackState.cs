using UnityEngine;
using Util;

public class BossAttackState : BossBaseState
{
    public BossAttackState(BossStateMachine sm) : base(sm) { }

    public override EBossState Key() => EBossState.Attack;

    public override EBossState NextState()
    {
        return EBossState.Idle;
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
