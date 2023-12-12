using System;
using System.Collections;
using System.Collections.Generic;
using UI.SubItem;
using UnityEngine;
using Util;

public class SkullController : MonoBehaviour
{
    [Header("Slave Component")] [SerializeField]
    private Collider2D skullCollider;

    [SerializeField] private Animator skullAnimator;
    [SerializeField] private SpriteRenderer equipRenderer;
    [SerializeField] private SpriteRenderer auraRenderer;

    [field: SerializeField] public Transform ActionSpawnPoint { get; private set; }

    [field: SerializeField] public HealthSystem Health { get; private set; }
    [field: SerializeField] public SlaveMasteryController MasteryController { get; private set; }
    [field: SerializeField] public SkullStateMachine SkullState { get; private set; }
    [field: SerializeField] public StatController Stat { get; private set; }

    public BuffSystem BuffSystem { get; private set; }
    public AuraSystem AuraSystem { get; private set; }

    [Header("Initial Skull")] public RuntimeAnimatorController originAnimator;

    public bool IsMastered { get; private set; } = false;
    public NewEquip CurrentEquip { get; private set; } = null;

    public event Action<NewEquip> OnEquipEvent;

    public event Action OnRecoveryEvent;
    private float _timeSinceControl = 0f;


    private void Awake()
    {
        BuffSystem = GetComponent<BuffSystem>();
        AuraSystem = GetComponent<AuraSystem>();
    }

    private void Start()
    {
        AuraSystem.AuraOnEvent += ShowAuraSprite;
        Health.OnHealEvent += ShowRecoveryText;
    }

    public void Recovery()
    {
        if (Health.CurrentHealth >= Health.MaxHealth)
        {
            return;
        }
        
        _timeSinceControl += Time.deltaTime;
        //TODO 연구소 내용 가져오기
        float healTime = Constants.Time.SkullBaseHealTime;
        if (_timeSinceControl >= healTime)
        {
            UI_Recovery recovery =
                ObjectPoolManager.Instance.GetGo(PoolType.RecoveryEffect).GetComponent<UI_Recovery>();
            recovery.transform.SetParent(Managers.GameManager.Player.transform, false);
            recovery.Initialize();

            _timeSinceControl = 0f;
            OnRecoveryEvent?.Invoke();
        }
    }

    public void SetMaster()
    {
        IsMastered = true;
        skullCollider.isTrigger = false;

        SkullState.ControlSlave();
        gameObject.SetActive(false);
    }

    public void ReleaseMaster(Vector2 position)
    {
        IsMastered = false;
        skullCollider.isTrigger = true;

        gameObject.transform.position = position;
        auraRenderer.gameObject.SetActive(AuraSystem.IsAuraOn);
        gameObject.SetActive(true);
        SkullState.ReleaseSlave();
    }

    public void SetEquip(NewEquip equip)
    {
        CurrentEquip = equip;
        equipRenderer.sprite = CurrentEquip.ArmedSprite;
        OnEquipEvent?.Invoke(equip);
    }

    public void ReleaseEquip(Vector2 dropPosition)
    {
        if (CurrentEquip == null)
            return;

        CurrentEquip.UnEquipped(dropPosition);
        CurrentEquip = null;
        equipRenderer.sprite = null;
        OnEquipEvent?.Invoke(null);
    }

    public Vector2 GetPosition()
    {
        return IsMastered ? Managers.GameManager.Player.transform.position : transform.position;
    }

    private void ShowRecoveryText(float recoveryHealth)
    {
        Vector3 position = GetPosition();
        UI_DamageText.ShowDamageText(recoveryHealth, Constants.Colors.RecoveryColor, position);
    }

    private void ShowAuraSprite()
    {
        auraRenderer.gameObject.SetActive(true);
    }
}