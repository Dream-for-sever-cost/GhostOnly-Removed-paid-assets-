using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public sealed class BuffSystem : MonoBehaviour
{
    public struct BuffTimer
    {
        public BuffModel Buff;
        public float TimeSinceAdded;
    }

    private List<BuffTimer> _buffs;
    public IReadOnlyList<BuffTimer> BuffTimers => _buffs;
    private HashSet<BuffModel.BuffType> _buffTypes;
    private List<BuffModel.BuffType> _removed;

    private StatController _statController;
    private HealthSystem _healthSystem;

    private UI_UnitStatus _uiUnitStatus;

    public delegate void BuffTimeElapsed(BuffModel buff, float elapsedTime);

    public event Action<BuffModel> AddBuffEvent;
    public event Action<BuffModel> RemoveBuffEvent;
    public event BuffTimeElapsed BuffTimeElapsedEvent;
    public event Action ClearBuffEvent;

    private void Awake()
    {
        _uiUnitStatus = GetComponentInChildren<UI_UnitStatus>();
        _statController = GetComponent<StatController>();
        _healthSystem = GetComponent<HealthSystem>();
        _buffs = new List<BuffTimer>(5);
        _buffTypes = new HashSet<BuffModel.BuffType>(5);
        _removed = new List<BuffModel.BuffType>(5);
    }

    private void Start()
    {
        _uiUnitStatus.SetBuffSystem(this);
        _healthSystem.OnDeathEvent += ClearBuffs;
    }

    private void ClearBuffs()
    {        
        for (int i = 0; i < _buffs.Count; i++)
        {
            _removed.Add(_buffs[i].Buff.Type);
        }
        
        foreach (BuffModel.BuffType buffType in _removed)
        {
            RemoveBuff(buffType);
        }
        
        _removed.Clear();
        ClearBuffEvent?.Invoke();
    }

    public void AddBuff(BuffModel buff)
    {
        if (_healthSystem.IsDeath()) { return;}
        if (_buffTypes.TryGetValue(buff.Type, out BuffModel.BuffType buffType))
        {
            int idx = _buffs.FindIndex((timer) => timer.Buff.Type == buffType);
            BuffTimer buffTimer = _buffs[idx];
            buffTimer.TimeSinceAdded = 0f;
            _buffs[idx] = buffTimer;
        }
        else
        {
            BuffTimer buffTimer = new BuffTimer { Buff = buff, TimeSinceAdded = 0f };
            _buffTypes.Add(buff.Type);
            _buffs.Add(buffTimer);
            foreach (StatType statType in buff.StatTypes)
            {
                if (statType == StatType.Attack)
                {
                    _statController.Stats[StatType.Strength].AddValue(buff.AddedValue);
                    _statController.Stats[StatType.Strength].MultipleValue(buff.MultipleValue);
                    _statController.Stats[StatType.Dexterity].AddValue(buff.AddedValue);
                    _statController.Stats[StatType.Dexterity].MultipleValue(buff.MultipleValue);
                    _statController.Stats[StatType.Intelligence].AddValue(buff.AddedValue);
                    _statController.Stats[StatType.Intelligence].MultipleValue(buff.MultipleValue);
                }
                else
                {
                    _statController.Stats[statType].AddValue(buff.AddedValue);
                    _statController.Stats[statType].MultipleValue(buff.MultipleValue);
                }
            }

            AddBuffEvent?.Invoke(buff);
        }
    }

    public void RemoveBuff(BuffModel.BuffType type)
    {
        int idx = _buffs.FindIndex((buffTimer) => buffTimer.Buff.Type == type);
        if (idx == -1)
        {
            Debug.Log($"failed to find :{type}");
            return;
        }

        BuffModel buff = _buffs[idx].Buff;

        foreach (StatType statType in buff.StatTypes)
        {
            if (statType == StatType.Attack)
            {
                _statController.Stats[StatType.Strength].AddValue(-buff.AddedValue);
                _statController.Stats[StatType.Strength].MultipleValue(-buff.MultipleValue);

                _statController.Stats[StatType.Dexterity].AddValue(-buff.AddedValue);
                _statController.Stats[StatType.Dexterity].MultipleValue(-buff.MultipleValue);

                _statController.Stats[StatType.Intelligence].AddValue(-buff.AddedValue);
                _statController.Stats[StatType.Intelligence].MultipleValue(-buff.MultipleValue);
            }
            else
            {
                _statController.Stats[statType].AddValue(-buff.AddedValue);
                _statController.Stats[statType].MultipleValue(-buff.MultipleValue);
            }
        }

        _buffs.RemoveAt(idx);
        _buffTypes.Remove(type);
        RemoveBuffEvent?.Invoke(buff);
    }

    public void AddTime(float deltaTime)
    {
        for (int i = 0; i < _buffs.Count; i++)
        {
            BuffTimer timer = _buffs[i];
            timer.TimeSinceAdded += deltaTime;
            BuffTimeElapsedEvent?.Invoke(timer.Buff, timer.TimeSinceAdded);
            _buffs[i] = timer;
            if (timer.TimeSinceAdded >= timer.Buff.LastingTime)
            {
                _removed.Add(timer.Buff.Type);
            }
        }

        foreach (BuffModel.BuffType type in _removed)
        {
            //todo cause destroyed exception  
            //파괴된 ui 객체에 접근할 수 있음 -> 수정 필요
            RemoveBuff(type);
        }

        _removed.Clear();
    }

    private void Update()
    {
        AddTime(Time.deltaTime);
    }
}