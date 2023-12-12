using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : MonoBehaviour
{
    private const int SOUL_REWARD = 2;
    private readonly int aniDeathHash = Animator.StringToHash("Die");

    private float currentDeathDuration = 1f;

    [field: Header("Hero Component")]
    [field: SerializeField] public Animator Ani { get; private set; }
    [field: SerializeField] public StatController Stat { get; private set; }
    [field: SerializeField] public HealthSystem Health { get; private set; }


    public void init()
    {
        Managers.Target.AddEnemy(transform);
        Health.TakeHeal(4);
    }

    private void Update()
    {
        if (Health.CurrentHealth <= 0)
        {
            currentDeathDuration -= Time.deltaTime;
            Ani.SetBool(aniDeathHash, true);
            Managers.Target.RemoveEnemy(transform);

            if (currentDeathDuration < 0)
            {
                Ani.SetBool(aniDeathHash, false);
                Managers.Soul.GetSoul(SOUL_REWARD);
                GetComponent<Collider2D>().isTrigger = false;

                gameObject.SetActive(false);
            }
        }
    }
}
