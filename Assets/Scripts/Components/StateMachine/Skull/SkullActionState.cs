using UnityEngine;

public class SkullActionState : SkullBaseState
{
    public SkullActionState(SkullStateMachine sm) : base(sm)
    {
    }

    public override ESkullState Key() => ESkullState.Action;

    public override ESkullState NextState()
    {
        ESkullState newState = base.NextState();
        if (newState != ESkullState.Action)
        {
            return newState;
        }

        if (!CanAction)
        {
            return ESkullState.Idle;
        }

        return base.NextState();
    }

    public override void Update()
    {
        base.Update();
        if (!CanAction) { return; }

        if (StateMachine.SkullController.CurrentEquip.Action.GetCooltimePercentage() >= 1)
        {
            HandleAction();
        }
    }


    private void HandleAction()
    {
        Vector2 dir = (StateMachine.Target.transform.position - StateMachine.transform.position).normalized;
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        StateMachine.Rotate(rotZ);
        StateMachine.SkullController.CurrentEquip.Action.Action(
            dir: dir,
            spawnPoint: StateMachine.ActionPosition.position,
            rotZ - 90);
        StateMachine.SetAnimationTrigger(ESkullState.Action);
    }
}