using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSpawnEnemy : EquipAction
{
    [field: Header("Extra Info")]
    [field: SerializeField] public PoolType SpawnObject { get; private set; }

    private void Reset()
    {
        Range = 0;
        Cooltime = 3;
    }

    public override bool Action(Vector2 dir, Vector2 spawnPoint, float rotZ)
    {
        if (!Able)
            return false;

        GameObject enemy = ObjectPoolManager.Instance.GetGo(SpawnObject);

        if (enemy.TryGetComponent(out HeroStateMachine hsm))
        {
            hsm.Initialize(spawnPoint);
        }

        Managers.Sound.PlaySound(SoundType);

        RecentTime = GetCooltime();
        Able = false;

        return true;
    }
}