using System;
using UnityEngine;
using Util;

public class VfxPoolable: PoolAble
{
    private float _timeSinceEnabled = 0f;
    private void OnEnable()
    {
        _timeSinceEnabled = 0f;
    }

    private void Update()
    {
        _timeSinceEnabled += Time.deltaTime;
        if (_timeSinceEnabled >= Constants.Time.AnimationTime)
        {
            ReleaseObject();
        }
    }
}