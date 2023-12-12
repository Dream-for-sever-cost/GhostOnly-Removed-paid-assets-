using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public bool IsDeath();

    public void TakeDamage(float damage);
}
