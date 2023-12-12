using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : PoolAble, IActionSpawnable
{
    private float healAmount = 20;
    private LayerMask targetLayer;
    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2f)
        {
            ReleaseObject();
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetLayer.value == (targetLayer.value | (1 << collision.gameObject.layer)))
        {
            if (collision.TryGetComponent(out HealthSystem obj))
            {
                if (obj.CurrentHealth > 0)
                    obj.TakeHeal(healAmount);
            }
        }
    }

    public void Initialize(LayerMask target)
    {
        targetLayer = target;
    }
}
