using System;
using UnityEngine;

public class SlaveRangeController : MonoBehaviour
{
    public event Action<GameObject> OnFindTargetEvent;
    public event Action OnDisappearTargetEvent;
    private SkullStateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = GetComponentInParent<SkullStateMachine>();
    }

    private void Update()
    {
        float followRange = _stateMachine.RangeOfFollow;
        Transform target =
            Managers.Target.GetNearestTarget(
                agent: _stateMachine.gameObject.transform,
                targetLayer: _stateMachine.TargetMask);
        if (target == null)
        {
            OnDisappearTargetEvent?.Invoke();
            return;
        }

        Vector2 distVector = target.position - _stateMachine.transform.position;
        bool canFollow = (distVector.x * distVector.x + distVector.y * distVector.y) <= followRange * followRange;

        if (canFollow)
        {
            OnFindTargetEvent?.Invoke(target.gameObject);
        }
        else
        {
            OnDisappearTargetEvent?.Invoke();
        }
    }
}