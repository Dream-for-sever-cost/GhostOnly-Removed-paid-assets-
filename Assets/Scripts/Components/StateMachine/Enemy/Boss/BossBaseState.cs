using UnityEngine;

public abstract class BossBaseState : IState<BossBaseState.EBossState>
{
    public enum EBossState
    {
        Idle,
        Move,
        Attack,
        Hit,
        Death,
        Concentrate,
        Skill,
    }

    protected BossStateMachine StateMachine;

    public BossBaseState(BossStateMachine sm) { StateMachine = sm; }

    public abstract EBossState Key();

    public virtual EBossState NextState()
    {
        if (StateMachine.IsHit) { return EBossState.Hit; }

        if (StateMachine.IsSkill) { return EBossState.Concentrate; }
        else if (StateMachine.IsAttack) { return EBossState.Attack; }

        if (StateMachine.IsMoving) { return EBossState.Move; }

        return EBossState.Idle;
    }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void OnTriggerEnter2D(Collider2D other) { }
    public virtual void OnTriggerExit2D(Collider2D other) { }
    public virtual void OnTriggerStay2D(Collider2D other) { }

}
