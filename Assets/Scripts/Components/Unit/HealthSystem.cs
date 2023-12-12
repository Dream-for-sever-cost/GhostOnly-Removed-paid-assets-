using DG.Tweening;
using System;
using TMPro;
using UI.SubItem;
using UnityEngine;
using UnityEngine.UI;
using Util;

[RequireComponent(typeof(StatController))]
public class HealthSystem : MonoBehaviour, IDamagable
{
    public float MaxHealth { get; private set; }
    [field: SerializeField] public float CurrentHealth { get; private set; }

    private StatController _statController;
    public event Action OnDeathEvent;
    public event Action<float> OnHealEvent;
    public event Action<float> OnGettingDamageEvent;
    public event Action OnDecreaseHealthEvent;

    private void Awake()
    {
        _statController = GetComponent<StatController>();
        _statController.InitialSetup();
    }

    private void OnEnable()
    {
        UpdateMaxHp();
    }

    private void OnDisable()
    {
        OnGettingDamageEvent = null;
        OnDeathEvent = null;
    }

    public void UpdateMaxHp()
    {
        MaxHealth = _statController.Stats[StatType.MaxHP].Value;
    }

    public bool IsDeath() => CurrentHealth <= 0;

    public void TakeDamage(float damage)
    {
        if (IsDeath()) { return; }

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        ShowDamageText(damage, Constants.Colors.HitColor);
        OnGettingDamageEvent?.Invoke(damage);

        if (IsDeath())
        {
            OnDeathEvent?.Invoke();

            Managers.Sound.PlaySound(Data.SoundType.Death, transform.position, true);
        }

        Managers.Sound.PlaySound(Data.SoundType.SkullHit, transform.position, true);
    }

    public void TakeHeal(float heal)
    {
        float prevHealth = CurrentHealth;
        CurrentHealth = Mathf.Min(CurrentHealth + heal, MaxHealth);

        if (CurrentHealth - prevHealth > 0)
        {
            OnHealEvent?.Invoke(heal);
        }
    }

    public void DecreaseHealth(float hpValue)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - hpValue);

        OnDecreaseHealthEvent?.Invoke();

        if (IsDeath())
        {
            OnDeathEvent?.Invoke();
        }
    }

    private void ShowDamageText(float damage, Color color)
    {
        UI_DamageText.ShowDamageText(damage, color, transform.position);
    }
}