using UnityEngine;

public abstract class SkullBaseState : IState<SkullBaseState.ESkullState>
{
    public enum ESkullState
    {
        Idle,
        Move,
        Action,
        Hit,
        Death
    }

    protected SkullStateMachine StateMachine;
    protected bool CanAction;
    protected bool IsDeath;


    public SkullBaseState(SkullStateMachine sm)
    {
        StateMachine = sm;
    }


    public abstract ESkullState Key();

    public virtual ESkullState NextState()
    {
        if (StateMachine.IsHit) { return ESkullState.Hit; }

        if (IsDeath) { return ESkullState.Death; }

        if (CanAction) { return ESkullState.Action; }

        return Key();
    }

    public virtual void Update()
    {
        UpdateActionAvailability();
        UpdateDeathState();
    }

    public virtual void FixedUpdate() { }

    public virtual void Enter()
    {
        StateMachine.SetAnimationTrigger(Key());
    }

    public virtual void Exit() { }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            StateMachine.Walls.Remove(other.transform);
        }
    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            StateMachine.Walls[other.transform] =  other;
        }
    }

    private void UpdateActionAvailability()
    {
        if (StateMachine.SkullController.CurrentEquip == null || StateMachine.Target == null)
        {
            CanAction = false;
            return;
        }

        float dist = Vector2.Distance(
            StateMachine.transform.position,
            StateMachine.Target.transform.position);
        CanAction = dist <= StateMachine.RangeOfAction;
    }

    private void UpdateDeathState()
    {
        IsDeath = StateMachine.HealthSystem.IsDeath();
    }
}