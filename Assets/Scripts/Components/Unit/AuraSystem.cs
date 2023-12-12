using Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using Util;


public class AuraSystem : MonoBehaviour
{
    [SerializeField] private LayerMask targetMask;
    private DataManager _dataManager;

    private const float TickAura = Constants.Time.TimePerAuraTick;
    public bool IsAuraOn { get; private set; }
    private float _timeSinceLastTick = 0f;
    private SkullController _skullController;
    private StatController _statController;
    private HealthSystem _healthSystem;
    private TargetManager _targetManager;
    private SlaveMasteryController _masteryController;

    private List<Aura> _auras;
    private Dictionary<Aura, BuffModel> _buffs;

    public event Action AuraOnEvent;
    //todo add aura ui 

    //todo add VFX Manager
    //----particle System 을 이용해서 할 것??

    private void Awake()
    {
        _skullController = GetComponent<SkullController>();
        _healthSystem = GetComponent<HealthSystem>();
        _masteryController = GetComponent<SlaveMasteryController>();
        _statController = GetComponent<StatController>();
        _targetManager = Managers.Target;
        _dataManager = Managers.Data;
        _auras = new List<Aura>();
        _buffs = new Dictionary<Aura, BuffModel>();
    }

    private void Start()
    {
        AddToAuraList(_masteryController.Effects);
        _masteryController.OnMainMasteryOpened += AddMasteryEffectToAuras;
        _masteryController.OnSubMasteryOpened += AddMasteryEffectToAuras;
    }

    private void AddMasteryEffectToAuras(Mastery mastery)
    {
        AddToAuraList(mastery.Effects);
    }

    private void AddToAuraList(IReadOnlyList<MasteryEffect> effects)
    {
        foreach (MasteryEffect effect in effects)
        {
            if (effect.Type != Mastery.MasteryEffectType.AuraEffect) { continue; }

            MasteryAuraEffect auraEffect = effect as MasteryAuraEffect;
            if (auraEffect == null) { throw new NullReferenceException("Type is not matched to class"); }

            //todo get aura from data manager
            Aura aura = auraEffect.AuraType switch
            {
                Aura.AuraType.SunFireAura => new Aura(
                    auraStatType: StatType.Strength,
                    type: auraEffect.AuraType,
                    auraRange: Constants.Distance.AuraRange,
                    effectBaseValue: 5f,
                    isDeBuff: true,
                    effectCoefficient: 0f
                ),
                Aura.AuraType.FrozenAura => new Aura(
                    auraStatType: StatType.Dexterity,
                    type: auraEffect.AuraType,
                    auraRange: Constants.Distance.AuraRange,
                    effectBaseValue: 2.0f,
                    isDeBuff: true,
                    effectCoefficient: 0f
                ),
                Aura.AuraType.CurseAura => new Aura(
                    auraStatType: StatType.Intelligence,
                    type: auraEffect.AuraType,
                    auraRange: Constants.Distance.AuraRange,
                    effectBaseValue: 5f,
                    isDeBuff: true,
                    effectCoefficient: 1.0f
                ),
                _ => throw new ArgumentOutOfRangeException()
            };
            _auras.Add(aura);
            BuffModel buff = ToBuffs(aura);
            _buffs[aura] = buff;
        }

        IsAuraOn = _auras.Count > 0;
        if (IsAuraOn)
        {
            AuraOnEvent?.Invoke();
        }
    }


    private void Update()
    {
        if (!IsAuraOn || _healthSystem.IsDeath()) { return; }

        AddTime(Time.deltaTime);
    }

    public void AddTime(float deltaTime)
    {
        _timeSinceLastTick += deltaTime;

        if (_timeSinceLastTick >= TickAura)
        {
            _timeSinceLastTick = 0f;
            HandleAuraToTargets();
        }
    }

    private void HandleAuraToTargets()
    {
        foreach (Aura aura in _auras)
        {
            LayerMask appliedTarget = aura.IsDeBuff ? targetMask : 1 << gameObject.layer;
            Transform[] targets = _targetManager.GetTargets(
                appliedTarget,
                GetPosition(),
                aura.AuraRange,
                out int lenOfTargets);

            for (int j = 0; j < lenOfTargets; j++)
            {
                if (!targets[j].TryGetComponent(out IDamagable damagable)) { continue; }

                if (damagable.IsDeath()) { continue; }

                BuffSystem buffSystem = targets[j].GetComponent<BuffSystem>();
                BuffModel buff = _buffs[aura];
                if (buff != null)
                {
                    buffSystem.AddBuff(buff);
                }

                //todo add vfx
                ShowVfx(targets[j].transform, aura.Type);
                if (aura.Type == Aura.AuraType.SunFireAura)
                {
                    float damage = GetAuraValue(aura, _statController.Stats);
                    damagable.TakeDamage(damage);
                }
            }
        }
    }

    private float GetAuraValue(Aura aura, IReadOnlyDictionary<StatType, Condition> stats)
    {
        return aura.EffectBaseValue + aura.EffectCoefficient * stats[aura.AuraStatType].Value;
    }

    private BuffModel ToBuffs(Aura aura)
    {
        int mark = aura.IsDeBuff ? -1 : 1;
        float appliedValue = GetAuraValue(aura, _statController.Stats) * mark;
        BuffModel ret;
        switch (aura.Type)
        {
            case Aura.AuraType.FrozenAura:
                ret = new BuffModel(
                    new[] { StatType.ActionDelay, StatType.MoveSpeed },
                    lastingTime: 1.5f,
                    addedValue: appliedValue,
                    BuffModel.BuffType.SlowDeBuff);
                break;
            case Aura.AuraType.CurseAura:
                ret = new BuffModel()
                {
                    AddedValue = appliedValue,
                    Type = BuffModel.BuffType.HorrorDeBuff,
                    LastingTime = 1f,
                    StatTypes = new[] { StatType.Attack }
                };
                break;
            default:
                ret = null;
                break;
        }

        return ret;
    }

    private Vector2 GetPosition()
    {
        return _skullController.IsMastered ? Managers.GameManager.Player.transform.position : transform.position;
    }

    private void ShowVfx(Transform parent, Aura.AuraType auraType)
    {
        PoolType poolType = auraType switch
        {
            Aura.AuraType.SunFireAura => PoolType.FireExplosionVfx,
            Aura.AuraType.FrozenAura => PoolType.FrozenVfx,
            Aura.AuraType.CurseAura => PoolType.HorrorVfx,
            _ => throw new ArgumentOutOfRangeException(nameof(auraType), auraType, null)
        };

        GameObject go = ObjectPoolManager.Instance.GetGo(poolType);
        go.transform.SetParent(parent, false);
    }
}