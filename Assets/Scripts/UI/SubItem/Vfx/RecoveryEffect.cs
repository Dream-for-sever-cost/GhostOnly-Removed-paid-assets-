using DG.Tweening;
using System;
using UnityEngine;
using Util;

public class UI_Recovery : PoolAble
{
    private bool _isReady;
    private float _timeSinceStarted = 0f;

    public void Initialize()
    {
        _isReady = true;
        _timeSinceStarted = 0f;
    }

    private void Update()
    {
        if (!_isReady) { return; }

        _timeSinceStarted += Time.deltaTime;

        if (_timeSinceStarted >= Constants.Time.AnimationTime)
        {
            ReleaseObject();
        }
    }
}