using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : PoolAble, IActionSpawnable
{
    private static readonly BuffModel SlowBuff =
        new BuffModel(new[] { StatType.MoveSpeed }, 5f, 0, -0.8f, BuffModel.BuffType.SlowMovementSpdHalf);

    private LayerMask targetLayer;
    private float _timer = 0;

    void Update()
    {
        _timer += Time.deltaTime;
        if (!(_timer >= 2))
        {
            return;
        }

        ReleaseObject();
        _timer = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetLayer.value != (targetLayer.value | (1 << collision.gameObject.layer)))
        {
            return;
        }

        if (collision.TryGetComponent(out BuffSystem buff))
        {
            buff.AddBuff(SlowBuff);
        }
    }

    public void Initialize(LayerMask target)
    {
        targetLayer = target;
    }
}