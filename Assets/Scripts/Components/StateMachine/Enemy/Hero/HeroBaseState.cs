using UnityEngine;

public abstract class HeroBaseState : IState<HeroBaseState.EHeroState>
{
    
    public enum EHeroState
    {
        Idle,
        Move,
        Attack,
        Hit,
        Death,
    }

    protected HeroStateMachine StateMachine;

    public HeroBaseState(HeroStateMachine sm) { StateMachine = sm; }

    public abstract EHeroState Key();

    public virtual EHeroState NextState()
    {
        if (StateMachine.IsHit) { return EHeroState.Hit; }

        if (StateMachine.IsAttack) { return EHeroState.Attack; }

        if (StateMachine.IsMoving) { return EHeroState.Move; }

        return EHeroState.Idle;
    }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void OnTriggerEnter2D(Collider2D other) { }
    public virtual void OnTriggerExit2D(Collider2D other) { }
    public virtual void OnTriggerStay2D(Collider2D other) { }
}
