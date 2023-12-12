using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System;
using System.Data;
using System.Linq;

public class StatController : MonoBehaviour
{
    [field: SerializeField] public UnitType UnitName { get; private set; }
    private SlaveMasteryController _masteryController;

    private Dictionary<StatType, Condition> _baseStats;
    public IReadOnlyDictionary<StatType, Condition> Stats => _baseStats;

    private void Awake()
    {
        _masteryController = GetComponent<SlaveMasteryController>();
    }

    private void Start()
    {
        if (_masteryController != null)
        {
            _masteryController.OnMainMasteryOpened += UpdateMasteryEffect;
            _masteryController.OnSubMasteryOpened += UpdateMasteryEffect;
            ApplyMasteryEffect(_masteryController.Effects);
        }
    }

    public void InitialSetup()
    {
        _baseStats = new Dictionary<StatType, Condition>(15);
        UnitStatData currentUnit = Managers.Data.UnitDic[UnitName.ToString()];
        
        _baseStats.Add(StatType.MaxHP, new Condition(currentUnit.maxHp, MaxStat.MAX_HP, MinStat.MIN_HP));
        _baseStats.Add(StatType.MoveSpeed, new Condition(currentUnit.moveSpeed, MaxStat.MOVESPEED, MinStat.MOVESPEED));
        _baseStats.Add(StatType.Strength, new Condition(currentUnit.strength, MaxStat.STRENGTH, MinStat.STRENGTH));
        _baseStats.Add(StatType.Dexterity, new Condition(currentUnit.dexterity, MaxStat.DEXTERITY, MinStat.DEXTERITY));
        _baseStats.Add(StatType.Intelligence,
            new Condition(currentUnit.intelligence, MaxStat.INTELLIGENCE, MinStat.INTELLIGENCE));
        _baseStats.Add(StatType.CriticalRate,
            new Condition(currentUnit.criticalRate, MaxStat.CRITICAL, MinStat.CRITICAL));
        _baseStats.Add(StatType.ActionDelay, new Condition(0f, MaxStat.ACTION_DELAY, MinStat.ACTION_DELAY));
    }

    private void ApplyMasteryEffect(IReadOnlyList<MasteryEffect> effects)
    {
        foreach (MasteryEffect effect in effects)
        {
            if (effect.Type != Mastery.MasteryEffectType.AddStatEffect) { continue; }

            if (effect is not MasteryAddStatEffect addStatEffect)
            {
                continue;
            }

            Stats[addStatEffect.StatType].AddValue(addStatEffect.AddedValue);
        }
    }

    private void UpdateMasteryEffect(Mastery mastery)
    {
        ApplyMasteryEffect(mastery.Effects);
    }
}