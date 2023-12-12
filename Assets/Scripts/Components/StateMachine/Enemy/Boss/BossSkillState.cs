using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class BossSkillState : BossBaseState
{
    public BossSkillState(BossStateMachine sm) : base(sm) { }

    public override EBossState Key() => EBossState.Skill;

    public override EBossState NextState()
    {
        return EBossState.Idle;
    }

    public override void Enter()
    {
        StateMachine.Ani.SetTrigger(Constants.AniParams.Action);
        StateMachine.Agent.Stop();
        Skill(StateMachine.Target);

        StateMachine.IsSkill = false;
    }

    private void Skill(Transform target)
    {
        if (target == null)
            return;

        Vector2 dir = (target.position - StateMachine.transform.position).normalized;
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        StateMachine.RotateEquip(rotZ);
        StateMachine.Skill.Action(dir, StateMachine.AttackSpawnPoint.position, rotZ - 90);
    }
}
