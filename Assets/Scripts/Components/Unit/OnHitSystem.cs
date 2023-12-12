using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

public sealed class OnHitSystem : MonoBehaviour
{
    private List<OnHit> _onHits;
    public IReadOnlyList<OnHit> OnHits => _onHits;

    private SkullController _skullController;
    private SlaveMasteryController _masteryController;
    private BuffSystem _buffSystem;
    private HealthSystem _healthSystem;

    private void Awake()
    {
        _skullController = GetComponent<SkullController>();
        _masteryController = GetComponent<SlaveMasteryController>();
        _healthSystem = GetComponent<HealthSystem>();
        _buffSystem = GetComponent<BuffSystem>();
        _onHits = new List<OnHit>(2);
    }

    private void Start()
    {
        _skullController.OnEquipEvent += ListeningActionEvent;
        AddOnHitEffect(_masteryController.Effects);
        _masteryController.OnMainMasteryOpened += UpdateOnHitsByMastery;
        _masteryController.OnSubMasteryOpened += UpdateOnHitsByMastery;

        //todo insert into database get from onHit id
    }

    private void AddOnHitEffect(IReadOnlyList<MasteryEffect> effects)
    {
        foreach (MasteryEffect effect in effects)
        {
            if (effect is not MasteryOnHitEffect onHitEffect)
            {
                continue;
            }

            OnHit onHit = Managers.Data.OnHitEntries[onHitEffect.OnHitType.ToString()];
            _onHits.Add(onHit);
        }
    }

    private void UpdateOnHitsByMastery(Mastery mastery)
    {
        AddOnHitEffect(mastery.Effects);
    }

    private void ListeningActionEvent(NewEquip equip)
    {
        if (equip == null) { return; }

        equip.Action.HitCallback = CreateOnHitEffects;
    }

    private void CreateOnHitEffects(Collider2D other)
    {
        Debug.Log("OnHitEffect called");
        foreach (OnHit onHit in _onHits)
        {
            float random = Random.Range(0, 1f);
            if (!(onHit.Percent > random))
            {
                continue;
            }

            PoolType vfx = OnHitToVfx(onHit.Type);
            ShowVfxOnPosition(other.transform.position, vfx);

            switch (onHit.Type)
            {
                case OnHit.OnHitType.RageOnHit:
                    GiveOnHitDamage(other, onHit);

                    //todo buff model insert into database
                    _buffSystem.AddBuff(new BuffModel(
                        type: BuffModel.BuffType.RageBuff,
                        statTypes: new[] { StatType.ActionDelay },
                        lastingTime: onHit.LastingTime,
                        addedValue: onHit.EffectAmount));
                    break;
                case OnHit.OnHitType.LifeStealOnHit:
                    GiveOnHitDamage(other, onHit);
                    _healthSystem.TakeHeal(onHit.EffectAmount);
                    break;
                case OnHit.OnHitType.ChainLightningOnHit:
                    CreateChainLightning(onHit, other.transform.position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void GiveOnHitDamage(Collider2D other, OnHit onHit)
    {
        HealthSystem healthSystem = other.GetComponent<HealthSystem>();
        if (onHit.DamageCoefficient > 0f && healthSystem != null)
        {
            float damage = _skullController.Stat.Stats[onHit.StatType].Value;
            healthSystem.TakeDamage(onHit.DamageCoefficient * damage);
        }
    }

    private void CreateChainLightning(OnHit onHit, Vector3 createdPosition)
    {
        Debug.Log($"Create chainLightning : {onHit.PoolType}");
        Enum.TryParse(onHit.PoolType, out PoolType enumPoolType);
        GameObject effect = ObjectPoolManager.Instance.GetGo(enumPoolType);
        ChainLightning chainLightning = effect.GetComponent<ChainLightning>();

        chainLightning.Initialize(
            target: _skullController.CurrentEquip.Action.Target,
            position: createdPosition,
            GetDamage(onHit: onHit),
            maxDistance: Constants.Distance.ChainLightningDistance,
            8
        );
    }

    private float GetDamage(OnHit onHit)
    {
        return onHit.DamageCoefficient * _skullController.Stat.Stats[onHit.StatType].Value;
    }

    private PoolType OnHitToVfx(OnHit.OnHitType onHitType)
    {
        return onHitType switch
        {
            OnHit.OnHitType.LifeStealOnHit => PoolType.LifeStealVfx,
            OnHit.OnHitType.RageOnHit => PoolType.RageVfx,
            OnHit.OnHitType.ChainLightningOnHit => PoolType.ElectricExplosionVfx,
            _ => throw new ArgumentOutOfRangeException(nameof(onHitType), onHitType, null)
        };
    }

    private void ShowVfxOnPosition(Vector3 position, PoolType vfx)
    {
        ObjectPoolManager.Instance
            .GetGo(vfx)
            .transform.position = position;
    }
}