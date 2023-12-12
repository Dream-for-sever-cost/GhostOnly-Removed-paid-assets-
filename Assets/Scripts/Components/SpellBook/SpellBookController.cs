using System;
using UnityEngine;
using Util;

public class SpellBookController : MonoBehaviour
{
    private InteractNPC _interact;
    private UI_SpellBook _ui;
    public event Action OnUnlockedSpell;
    public event Action OnActivatedSpell;

    private void Awake()
    {
        _interact = GetComponent<InteractNPC>();
    }

    private void Start()
    {
        _interact.EventInteract.AddListener(ShowSpellBookPopup);
    }

    public void ShowSpellBookPopup()
    {
        _ui = Managers.UI.ShowPopupUI<UI_SpellBook>();
        Time.timeScale = 0;
        Managers.Sound.PlaySound(Data.SoundType.Interaction);
    }

    public void UpdateEffect(string spellId)
    {
        Managers.SpellBook.effect = Managers.SpellBook.SpellDataDic[spellId].effect;
    }

    public void UnlockSpell(string spellId)
    {
        Managers.SpellBook.SpellDataDic[spellId].isActivated = true;
        Managers.SpellBook.SpellDataDic[spellId].isLocked = false;
    }

    public void ActivateSpell(string spellId)
    {
        Invoke(Managers.SpellBook.SpellDataDic[spellId].spellType.ToString(), 0f);
    }

    // 해골 기력 회복 양 증가
    private void IncreaseSkullEnergyRecoverySpeed()
    {
        Managers.GameManager.recoverySpeed += Managers.SpellBook.effect;
        CancelInvoke(Constants.Spellbook.IncreaseSkullEnergyRecoverySpeed);
    }

    // 해골 최대수 증가
    private void IncreaseMaxSkullCount()
    {
        Managers.GameManager.ChangeSkullCount(ref Managers.GameManager.maxSkullCount, (int)Mathf.Round(Managers.SpellBook.effect));
        CancelInvoke(Constants.Spellbook.IncreaseMaxSkullCount);
    }

    // 해골의 모든 데미지 증가
    private void IncreaseSkullDamage()
    {
        Managers.GameManager.attckPowerCoefficient += Managers.SpellBook.effect;
        Managers.SlaveManager.NotifyStatChanged(StatType.Strength);
        Managers.SlaveManager.NotifyStatChanged(StatType.Dexterity);
        Managers.SlaveManager.NotifyStatChanged(StatType.Intelligence);
        CancelInvoke(Constants.Spellbook.IncreaseSkullDamage);
    }

    // 해골의 최대 체력 증가
    private void IncreaseSkullMaxHp()
    {
        Managers.GameManager.skullMaxHpCoefficient = Managers.SpellBook.effect;
        Managers.SlaveManager.NotifyStatChanged(StatType.MaxHP);
        CancelInvoke(Constants.Spellbook.IncreaseSkullMaxHp);
    }

    // 해골 전체 이동 속도 증가
    private void IncreaseSkullMoveSpeed()
    {
        Managers.GameManager.skullMoveSpeedCoefficient = Managers.SpellBook.effect;
        Managers.SlaveManager.NotifyStatChanged(StatType.MoveSpeed);
        CancelInvoke(Constants.Spellbook.IncreaseSkullMoveSpeed);
    }

    // 유령 이동 속도 증가
    private void IncreaseGhostMoveSpeed()
    {
        Managers.GameManager.ghostMoveSpeedCoefficient += Managers.SpellBook.effect;
        CancelInvoke(Constants.Spellbook.IncreaseGhostMoveSpeed);
    }

    // 묘비 재생성 시간 감소
    private void ReduceGravestoneRespawnTime()
    {
        Managers.GameManager.gravestoneRespawnTimeCoefficient += Managers.SpellBook.effect;
        CancelInvoke(Constants.Spellbook.ReduceGravestoneRespawnTime);
    }
}