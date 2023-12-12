using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<EState> where EState : Enum
{
    public EState Key();
    public EState NextState();
    public void Update();
    public void FixedUpdate();
    public void Enter();
    public void Exit();
    public void OnTriggerEnter2D(Collider2D other);
    public void OnTriggerExit2D(Collider2D other);
    public void OnTriggerStay2D(Collider2D other);
}