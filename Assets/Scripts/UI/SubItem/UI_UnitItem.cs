using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitItem : UI_Base
{
    private enum Images
    {
        UnitImage
    }

    private Image _unitImage;
    private SkullStateMachine _skull;

    public override bool Init()
    {
        if (_init) { return true; }

        BindImage(typeof(Images));
        _unitImage = GetImage((int)Images.UnitImage);
        _init = true;
        return true;
    }

    private void OnDestroy()
    {
        UnSubscribeEvent();
    }

    public void SetSkull(SkullStateMachine sm)
    {
        Init();
        if (_skull != null)
        {
            Debug.LogError("skull is already set");
            return;
        }

        _skull = sm;
        if (_skull != null)
        {
            SubscribeEvent();
        }

        UpdateUI();
        Debug.Log("Set skull");
    }

    public void SetOnClickListener(Action<SkullStateMachine> onClick)
    {
        _unitImage.BindEvent(action: () => onClick?.Invoke(_skull));
    }

    private void UpdateUI()
    {
        gameObject.SetActive(_skull != null);
        if (_skull == null) { return; }

        Sprite sprite = _skull.HealthSystem.IsDeath()
            ? Resources.Load<Sprite>("Sprites/Coffin/Skull")
            : Resources.Load<Sprite>("Sprites/Coffin/Skull_1");
        _unitImage.sprite = sprite;

        //todo color change ...
        float ratio = _skull.HealthSystem.MaxHealth == 0
            ? 0
            : _skull.HealthSystem.CurrentHealth / _skull.HealthSystem.MaxHealth;
        Color color = Color.Lerp(Color.red, Color.white, ratio);
        _unitImage.color = color;
    }

    private void UpdateUIByHeal(float obj)
    {
        UpdateUI();
    }

    private void UpdateUIByDamaged(float obj)
    {
        UpdateUI();
    }

    private void SubscribeEvent()
    {
        _skull.HealthSystem.OnDecreaseHealthEvent += UpdateUI;
        _skull.HealthSystem.OnGettingDamageEvent += UpdateUIByDamaged;
        _skull.HealthSystem.OnHealEvent += UpdateUIByHeal;
    }

    private void UnSubscribeEvent()
    {
        if (_skull == null) { return; }

        _skull.HealthSystem.OnDecreaseHealthEvent -= UpdateUI;
        _skull.HealthSystem.OnGettingDamageEvent -= UpdateUIByDamaged;
        _skull.HealthSystem.OnHealEvent -= UpdateUIByHeal;
    }
}