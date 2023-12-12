using UnityEngine;

public class SkullMovingState : SkullBaseState
{
    public SkullMovingState(SkullStateMachine sm) : base(sm)
    {
    }

    public override ESkullState Key() => ESkullState.Move;

    public override ESkullState NextState()
    {
        ESkullState newState = base.NextState();
        if (newState != Key())
        {
            return ESkullState.Idle;
        }

        if (StateMachine.Target != null && !IsArrive())
        {
            return newState;
        }

        return ESkullState.Idle;
    }

    public override void Update()
    {
        base.Update();
        if (StateMachine.Target == null) { return; }

        Vector2 dest = StateMachine.Target.transform.position;
        Vector2 dir = (dest - (Vector2)StateMachine.transform.position).normalized;
        float deg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        StateMachine.Rotate(deg);
        StateMachine.SetDirection(dir);
    }

    public override void Exit()
    {
        base.Exit();
        StateMachine.ToggleAnimationBool(Key());
    }

    private bool IsArrive()
    {
        Vector2 current = StateMachine.transform.position;
        Vector3 dest = StateMachine.Target.transform.position;
        float diffX = current.x - dest.x;
        float diffY = current.y - dest.y;
        diffX *= diffX;
        diffY *= diffY;

        float distPow = diffX + diffY;
        return distPow <= StateMachine.RangeOfAction * StateMachine.RangeOfAction;
    }
}