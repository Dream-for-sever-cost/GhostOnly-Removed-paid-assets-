using System;
using System.Collections.Generic;
using System.Linq;
using UI.SubItem;
using UnityEngine;

public sealed class SlaveMasteryController : MonoBehaviour
{
    private MasteryManager _masteryManager;
    private SoulManager _soulManager;

    private static StatType[] _addableStatTypes =
        new StatType[] { StatType.Strength, StatType.Dexterity, StatType.Intelligence };

    private StatController _statController;
    private SkullController _skullController;
    private Dictionary<StatType, int> _costs;
    public Mastery MainMastery { get; private set; }
    public Mastery SubMastery { get; private set; }

    private List<MasteryEffect> _effects = new List<MasteryEffect>(4);
    public IReadOnlyList<MasteryEffect> Effects => _effects;
    public bool IsMainMasteryOpened => MainMastery != Mastery.None;
    public bool IsSubMasteryOpened => SubMastery != Mastery.None;

    public bool SubMasteryOpen;

    public bool MainMasteryOpen;

    public bool MainMasteryLockOpen;

    public bool SubMasteryLockOpen;

    private UI_UnitMastery _masteryUI;

    public delegate void ChangeStatsState(
        StatType statType,
        bool isMainOpened,
        bool isSubOpened,
        int currentStats,
        int price);

    public event ChangeStatsState OnBuySuccess;
    public event Action<Mastery> OnMainMasteryOpened;
    public event Action<Mastery> OnSubMasteryOpened;
    public event Action OnFailedToBuyEvent;

    private void Awake()
    {
        _masteryManager = Managers.Mastery;
        _soulManager = Managers.Soul;
        _costs = new Dictionary<StatType, int>();
        _skullController = GetComponent<SkullController>();
        _statController = GetComponent<StatController>();
        MainMastery = Mastery.None;
        SubMastery = Mastery.None;
        _masteryUI = GetComponentInChildren<UI_UnitMastery>();
        Debug.Assert(_statController != null);
    }

    private void Start()
    {
        foreach (StatType statType in _addableStatTypes)
        {
            _costs[statType] = _masteryManager.GetCost(_statController.Stats[statType].Value.ToInt());
        }
    }

    public UI_Mastery ShowMasteryPopup()
    {
        Managers.Sound.PlaySound(Data.SoundType.Interaction);
        UI_Mastery masteryUI = Managers.UI.ShowPopupUI<UI_Mastery>();

        int strStats = (int)Math.Floor(_statController.Stats[StatType.Strength].BaseValue);
        int dexStats = (int)Math.Floor(_statController.Stats[StatType.Dexterity].BaseValue);
        int intStats = (int)Math.Floor(_statController.Stats[StatType.Intelligence].BaseValue);

        List<UI_Mastery.StatusInfo> infos = GetStatusInfos();

        Sprite weaponSprite = _skullController.CurrentEquip == null ? null : _skullController.CurrentEquip.ArmedSprite;
        masteryUI.UpdateUIState(MainMastery,
            SubMastery,
            isMainOpened: IsMainMasteryOpened,
            isSubOpened: IsSubMasteryOpened,
            strStats: strStats,
            dexStats: dexStats,
            intStats: intStats,
            weaponSprite: weaponSprite,
            costs: _costs,
            masteryController: this,
            effects: Effects,
            infos: infos);

        return masteryUI;
    }

    public List<UI_Mastery.StatusInfo> GetStatusInfos()
    {
        List<UI_Mastery.StatusInfo> infos = new List<UI_Mastery.StatusInfo>();
        Array statusTypes = Enum.GetValues(typeof(UI_Mastery.StatusType));
        foreach (UI_Mastery.StatusType statusType in statusTypes)
        {
            UI_Mastery.StatusInfo info = new UI_Mastery.StatusInfo()
            {
                Header = ToStatusHeader(statusType),
                ValueString = ToStatusValueString(statusType),
                AddedValueString = ToStatusAddedValueString(statusType),
            };
            infos.Add(info);
        }

        return infos;
    }

    public bool AddStandardMainMastery()
    {
        int[] stats = new int[3];
        stats[0] = _statController.Stats[StatType.Strength].BaseValue.ToInt();
        stats[1] = _statController.Stats[StatType.Dexterity].BaseValue.ToInt();
        stats[2] = _statController.Stats[StatType.Intelligence].BaseValue.ToInt();
        int max = int.MinValue;
        int maxIdx = -1;
        for (int i = 0; i < 3; i++)
        {
            if (max < stats[i])
            {
                max = stats[i];
                maxIdx = i;
            }
        }

        MainMastery = _masteryManager.CreateStandardMastery((Mastery.MasteryType)maxIdx);
        UpdateMasteryEffects();
        OnMainMasteryOpened?.Invoke(MainMastery);
        return true;
    }

    public bool AddStandardSubMastery()
    {
        int[] stats = new int[3];
        stats[0] = _statController.Stats[StatType.Strength].BaseValue.ToInt();
        stats[1] = _statController.Stats[StatType.Dexterity].BaseValue.ToInt();
        stats[2] = _statController.Stats[StatType.Intelligence].BaseValue.ToInt();
        int max = int.MinValue;
        int maxIdx = -1;
        for (int i = 0; i < 3; i++)
        {
            if (max < stats[i])
            {
                max = stats[i];
                maxIdx = i;
            }
        }

        SubMastery = _masteryManager.CreateStandardMastery((Mastery.MasteryType)maxIdx);
        UpdateMasteryEffects();
        OnSubMasteryOpened?.Invoke(SubMastery);
        return true;
    }

    public bool AddRandomMainMastery()
    {
        int strStat = _statController.Stats[StatType.Strength].BaseValue.ToInt();
        int dexStat = _statController.Stats[StatType.Dexterity].BaseValue.ToInt();
        int intStat = _statController.Stats[StatType.Intelligence].BaseValue.ToInt();
        MainMastery = _masteryManager.CreateRandomMastery(
            strStats: strStat,
            dexStats: dexStat,
            intStats: intStat,
            MasteryManager.EMasteryOpenType.Main);
        UpdateMasteryEffects();
        OnMainMasteryOpened?.Invoke(MainMastery);
        return true;
    }

    public bool AddRandomSubMastery()
    {
        int strStat = _statController.Stats[StatType.Strength].BaseValue.ToInt();
        int dexStat = _statController.Stats[StatType.Dexterity].BaseValue.ToInt();
        int intStat = _statController.Stats[StatType.Intelligence].BaseValue.ToInt();
        SubMastery = _masteryManager.CreateRandomMastery(
            strStats: strStat,
            dexStats: dexStat,
            intStats: intStat,
            mainMastery: MainMastery,
            MasteryManager.EMasteryOpenType.Sub);
        UpdateMasteryEffects();
        OnSubMasteryOpened?.Invoke(SubMastery);
        return true;
    }

    private void UpdateMasteryEffects()
    {
        List<MasteryEffect> effects = MainMastery.Effects.ToList();
        effects.AddRange(SubMastery.Effects);
        _masteryUI.SetMasteryArray(MainMastery, SubMastery);
        AddToEffect(effects);
    }

    private void AddToEffect(List<MasteryEffect> effects)
    {
        _effects.Clear();
        Dictionary<StatType, MasteryAddStatEffect> effectValues = new Dictionary<StatType, MasteryAddStatEffect>();
        foreach (MasteryEffect effect in effects)
        {
            switch (effect.Type)
            {
                case Mastery.MasteryEffectType.AddStatEffect:
                    if (effect is MasteryAddStatEffect addStatEffect)
                    {
                        if (effectValues.ContainsKey(addStatEffect.StatType))
                        {
                            effectValues[addStatEffect.StatType].AddedValue += addStatEffect.AddedValue;
                            effectValues[addStatEffect.StatType].PercentageValue += addStatEffect.PercentageValue;
                        }
                        else
                        {
                            effectValues[addStatEffect.StatType] = new MasteryAddStatEffect(
                                addStatEffect.Type,
                                addStatEffect.StatType,
                                addStatEffect.AddedValue,
                                addStatEffect.PercentageValue);
                        }
                    }

                    break;
                case Mastery.MasteryEffectType.None:
                    break;
                case Mastery.MasteryEffectType.OnHitEffect:
                    _effects.Add(effect);
                    Debug.Log("OnHit Effect");
                    break;
                case Mastery.MasteryEffectType.AuraEffect:
                    _effects.Add(effect);
                    break;
            }
        }


        foreach ((StatType type, MasteryAddStatEffect addStatEffect)in effectValues)
        {
            _effects.Add(addStatEffect);
        }
    }

    public bool CanOpenMainMastery()
    {
        int strStats = _statController.Stats[StatType.Strength].BaseValue.ToInt();
        int dexStats = _statController.Stats[StatType.Dexterity].BaseValue.ToInt();
        int intStats = _statController.Stats[StatType.Intelligence].BaseValue.ToInt();
        return _masteryManager.CanCreateMastery(
            strStats: strStats,
            dexStats: dexStats,
            intStats: intStats,
            openType: MasteryManager.EMasteryOpenType.Main);
    }

    public bool CanOpenSubMastery()
    {
        int strStats = _statController.Stats[StatType.Strength].BaseValue.ToInt();
        int dexStats = _statController.Stats[StatType.Dexterity].BaseValue.ToInt();
        int intStats = _statController.Stats[StatType.Intelligence].BaseValue.ToInt();
        return _masteryManager.CanCreateMastery(
            strStats: strStats,
            dexStats: dexStats,
            intStats: intStats,
            openType: MasteryManager.EMasteryOpenType.Sub);
    }

    public void BuyStats(StatType statsType)
    {
        int cost = _costs[statsType];
        bool canBuy = _soulManager.CheckSoul(cost) && !_statController.Stats[statsType].IsMax;

        if (canBuy)
        {
            Managers.Sound.PlaySound(Data.SoundType.Purchase);
            int newStats = (_statController.Stats[statsType].BaseValue.ToInt() + 1);
            _soulManager.UseSoul(cost);
            _statController.Stats[statsType].SetValue(newStats);
            _costs[statsType] = _masteryManager.GetCost(newStats);
            OnBuySuccess?.Invoke(
                statsType,
                CanOpenMainMastery(),
                CanOpenSubMastery(),
                _statController.Stats[statsType].BaseValue.ToInt(),
                cost);
        }
        else
        {
            Managers.Sound.PlaySound(Data.SoundType.PurchaseFail);
            OnFailedToBuyEvent?.Invoke();
        }

        Debug.Log($"BuyStats - statsType: {statsType}, result : {canBuy}");
    }

    private string ToStatusHeader(UI_Mastery.StatusType type)
    {
        return type switch
        {
            UI_Mastery.StatusType.Atk => "atk",
            UI_Mastery.StatusType.MaxHp => "hp",
            UI_Mastery.StatusType.CriticalRate => "critical",
            UI_Mastery.StatusType.MovementSpd => "movement_speed",
            UI_Mastery.StatusType.ActionDelay => "attack_speed",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private string ToStatusValueString(UI_Mastery.StatusType type)
    {
        if (type == UI_Mastery.StatusType.Atk)
        {
            bool isEquipped = _skullController.CurrentEquip == null;
            if (isEquipped) { return "0"; }

            StatType atkStatType = _skullController.CurrentEquip.Action.AppliedStat;
            if (atkStatType == StatType.MaxHP) { return "0"; }

            return _statController.Stats[atkStatType].BaseValue.ToString("N0");
        }

        float actionDelay = _skullController.CurrentEquip == null ? 0 : _skullController.CurrentEquip.Action.Cooltime;

        return type switch
        {
            UI_Mastery.StatusType.MaxHp => _statController.Stats[StatType.MaxHP].BaseValue.ToString("N0"),
            UI_Mastery.StatusType.CriticalRate => _statController.Stats[StatType.CriticalRate].BaseValue.ToString("N0"),
            UI_Mastery.StatusType.MovementSpd => _statController.Stats[StatType.MoveSpeed].BaseValue.ToString("N1"),
            UI_Mastery.StatusType.ActionDelay => actionDelay.ToString("N1"),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private string ToStatusAddedValueString(UI_Mastery.StatusType type)
    {
        if (type == UI_Mastery.StatusType.Atk)
        {
            bool isEquipped = _skullController.CurrentEquip == null;
            if (isEquipped) { return string.Empty; }

            StatType atkStatType = _skullController.CurrentEquip.Action.AppliedStat;
            if (atkStatType == StatType.MaxHP) { return string.Empty; }

            if (_statController.Stats[atkStatType].AddedValue == 0)
            {
                return string.Empty;
            }
            else if (_statController.Stats[atkStatType].AddedValue > 0)
            {
                return $" (+{_statController.Stats[atkStatType].AddedValue:N0})";
            }

            return $" ({_statController.Stats[atkStatType].AddedValue:N0})";
        }


        float value = type switch
        {
            UI_Mastery.StatusType.MovementSpd => _statController.Stats[StatType.MoveSpeed].AddedValue,
            UI_Mastery.StatusType.ActionDelay => _statController.Stats[StatType.ActionDelay].AddedValue,
            UI_Mastery.StatusType.MaxHp => _statController.Stats[StatType.MaxHP].AddedValue,
            UI_Mastery.StatusType.CriticalRate => _statController.Stats[StatType.CriticalRate].AddedValue,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        if (value == 0)
        {
            return string.Empty;
        }

        if (type == UI_Mastery.StatusType.CriticalRate)
        {
            return value > 0 ? $" (+{value:N0})" : $" ({value:N0})";
        }

        if (type == UI_Mastery.StatusType.MaxHp)
        {
            return value > 0 ? $" (+{value:N0})" : $" ({value:N0})";
        }

        return value > 0 ? $" (+{value:N1})" : $" ({value:N1})";
    }
}