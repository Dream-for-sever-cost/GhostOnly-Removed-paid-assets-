using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, IState<EState>> _states;
    public IState<EState> CurrentState { get; protected set; }
    public IState<EState> BaseState { get; protected set; }

    protected virtual void Awake()
    {
        _states = new Dictionary<EState, IState<EState>>();
    }

    protected virtual void Update()
    {
        CurrentState.Update();
        EState newState = CurrentState.NextState();

        if (!newState.Equals(CurrentState.Key()))
        {
            TransitionTo(_states[newState]);
        }
    }

    protected virtual void FixedUpdate()
    {
        CurrentState.FixedUpdate();
    }

    protected virtual void TransitionTo(IState<EState> newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        CurrentState.OnTriggerEnter2D(other);
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        CurrentState.OnTriggerStay2D(other);
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        CurrentState.OnTriggerExit2D(other);
    }
}