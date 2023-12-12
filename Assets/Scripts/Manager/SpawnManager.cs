using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System;
using Random = UnityEngine.Random;
using Util;

public class SpawnManager : MonoBehaviour
{
    [Serializable]
    public struct SpawnStruct
    {
        public Transform transform;
        public GameObject icon;
    }

    private static SpawnManager _instance;

    public static SpawnManager Instance
    {
        get
        {
            if (_instance != null) { return _instance; }

            _instance = FindObjectOfType<SpawnManager>();
            if (_instance != null) { return _instance; }

            return _instance = new GameObject(nameof(SpawnManager)).AddComponent<SpawnManager>();
        }
    }

    [SerializeField] private List<SpawnStruct> spawnStructs = new List<SpawnStruct>();

    private int[] spawnIndex;

    private int waveIndex = 1;

    public void Init()
    {
        spawnIndex = new int[3] { 0, 1, 2 };

        SetRandomSpawnIndex();

        StartCoroutine(CoAddSpawnEvent());
    }

    private IEnumerator CoAddSpawnEvent()
    {
        while (DayManager.Instance == null) { yield return null; }

        DayManager.Instance.OnChangedDayStatus += (bool isNight) =>
        {
            if (!isNight)
                StartWave();
            else
            {
                waveIndex = Mathf.Min(waveIndex + 1, 28);
                SetRandomSpawnIndex();
            }
        };
    }

    private void StartWave()
    {
        for (int i = 0; i < Managers.Data.WaveDataDic[waveIndex].roads.Count; i++)
        {
            StartCoroutine(CoSpawn(i, Managers.Data.WaveDataDic[waveIndex].roads[i]));
        }

        foreach (SpawnStruct ss in spawnStructs)
        {
            ss.icon.SetActive(false);
        }
    }

    private IEnumerator CoSpawn(int index, Road road)
    {
        foreach (RoadUnitData data in road.roadUnitInfoDic)
        {
            for (int j = 0; j < data.count; j++)
            {
                GameObject hero = ObjectPoolManager.Instance.GetGo(SwitchType(data.type));
                hero.GetComponent<IEnemyInitializer>().Initialize(spawnStructs[spawnIndex[index]].transform.position);
                yield return Constants.Delay.Spawn;
            }
        }
    }

    private void SetRandomSpawnIndex()
    {
        for (int i = 0; i < spawnIndex.Length; i++)
        {
            int randomIndex = Random.Range(0, spawnIndex.Length);

            (spawnIndex[i], spawnIndex[randomIndex]) = (spawnIndex[randomIndex], spawnIndex[i]);
        }

        foreach (SpawnStruct ss in spawnStructs)
        {
            ss.icon.SetActive(false);
        }

        for (int i = 0; i < Managers.Data.WaveDataDic[waveIndex].roads.Count; i++)
        {
            spawnStructs[spawnIndex[i]].icon.SetActive(Managers.Data.WaveDataDic[waveIndex].roads[i].roadUnitInfoDic.Count > 0);
        }
    }

    private PoolType SwitchType(UnitType type)
    {
        return Enum.TryParse(type.ToString(), out PoolType ret) ? ret : PoolType.SwordMan;
    }
}