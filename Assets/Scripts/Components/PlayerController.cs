using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;
using UI.SubItem;
using Util;

public class PlayerController : MonoBehaviour
{
    [Header("Player Component")] [SerializeField]
    private SpriteRenderer playerRenderer;

    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer equipRenderer;
    [SerializeField] private SpriteRenderer auraRenderer;
    [SerializeField] private Transform equipTransform;
    [SerializeField] private Transform actionSpawnPoint;
    [SerializeField] private GameObject spotlight;
    [SerializeField] private UI_UnitStatus uiUnitStatus;

    [Header("Initial Player")] [SerializeField]
    private RuntimeAnimatorController originAnimator;

    public SkullController CurrentSkull { get; private set; } = null;

    private float _moveSpeed = Constants.Speed.GhostInitial;
    private Vector2 _moveInput = Vector2.zero;
    private Vector2 _mousePos = Vector2.zero;
    private Vector2 _playerAim = Vector2.zero;
    private float _rotZ = 0f;

    private SkullController _collideSkull = null;
    private NewEquip _collideEquip = null;
    private InteractNPC _collideNPC = null;

    private UI_Game _ui;
    private UI_Mastery _uiMastery;
    private UI_UnitMastery _unitMastery;
    private MinimapCameraController _minimapCam;

    public event Action<bool> ControlSkullStartedEvent;

    private void Awake()
    {
        _ui = GameObject.Find(Constants.GameObjects.UI_Game).GetComponent<UI_Game>();
        _unitMastery = GetComponentInChildren<UI_UnitMastery>();
        Managers.GameManager.Player = this;
    }

    private void Start()
    {
        uiUnitStatus.gameObject.SetActive(false);
        DayManager.Instance.OnChangedDayStatus += (bool isNight) => { spotlight.SetActive(isNight); };
    }

    private void Update()
    {
        if (CurrentSkull == null) { return; }

        CurrentSkull.BuffSystem.AddTime(Time.deltaTime);
        CurrentSkull.AuraSystem.AddTime(Time.deltaTime);
        CurrentSkull.Recovery();

        if (CurrentSkull.CurrentEquip == null) { return; }

        _ui.UpdateCooltimeUI(CurrentSkull.CurrentEquip);
    }

    private void FixedUpdate()
    {
        playerRigidbody.velocity = Vector2.Lerp(playerRigidbody.velocity, _moveInput, 0.25f);
    }

    private void SetMoveSpeed()
    {
        if (Managers.UI.IsPopupUI())
        {
            _moveSpeed = 0f;
        }
        else if (CurrentSkull == null)
        {
            _moveSpeed = Constants.Speed.GhostInitial + Managers.GameManager.ghostMoveSpeedCoefficient;
        }
        else
        {
            _moveSpeed = CurrentSkull.Stat.Stats[StatType.MoveSpeed].Value;
        }
    }

    #region PlayerInput

    private void OnMove(InputValue value)
    {
        SetMoveSpeed();

        _moveInput = value.Get<Vector2>().normalized * _moveSpeed;

        playerAnimator.SetBool(Constants.AniParams.Move, _moveInput != Vector2.zero);
    }

    private void OnAim(InputValue value)
    {
        if (Managers.UI.IsPopupUI()) { return; }

        _mousePos = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        _playerAim = (_mousePos - (Vector2)transform.position).normalized;

        if (_playerAim.magnitude >= 0.5f)
        {
            _rotZ = Mathf.Atan2(_playerAim.y, _playerAim.x) * Mathf.Rad2Deg;
            playerRenderer.flipX = Mathf.Abs(_rotZ) < 90f;
            equipRenderer.flipY = Mathf.Abs(_rotZ) < 90f;
            equipTransform.rotation = Quaternion.Euler(0, 0, _rotZ + 180);
        }
    }

    private void OnPossess(InputValue value)
    {
        if (Managers.UI.IsPopupUI()) { return; }

        if (CurrentSkull != null)
        {
            //먼저 실행해서 이벤트 제거하도록 하기위함
            ControlSkullStartedEvent?.Invoke(false);
            ReleaseSkull();
        }
        else if (_collideSkull != null)
        {
            //나중에 실행해서 이벤트 등록하도록 하기위함
            SetSkull(_collideSkull);
            ControlSkullStartedEvent?.Invoke(true);
        }

        SetMoveSpeed();
    }

    private void OnEquip()
    {
        if (Managers.UI.IsPopupUI()) { return; }

        if (CurrentSkull?.CurrentEquip != null)
        {
            ReleaseEquip();
        }

        if (_collideEquip != null)
        {
            SetEquip(_collideEquip);
        }
    }

    private void OnInteract(InputValue value)
    {
        if (Managers.UI.IsPopupUI())
        {
            if (Managers.UI.FindPopup<UI_Coffin>())
            {
                Managers.UI.ClosePopupUI();
            }

            if (Managers.UI.FindPopup<UI_SpellBook>())
            {
                Managers.UI.ClosePopupUI();
            }

            return;
        }

        if (_collideNPC != null)
        {
            _collideNPC.EventInteract?.Invoke();
        }
    }

    private void OnAction(InputValue value)
    {
        if (Managers.UI.IsPopupUI()) { return; }

        if (CurrentSkull != null && CurrentSkull.CurrentEquip != null)
        {
            if (CurrentSkull.CurrentEquip.Action.Action(_playerAim, actionSpawnPoint.position, _rotZ - 90))
            {
                //playerAnimator.SetTrigger(Constants.AniParams.Action);
            }
        }
    }

    private void OnSkill(InputValue value)
    {
        if (Managers.UI.IsPopupUI()) { return; }

        if (CurrentSkull != null && CurrentSkull.CurrentEquip != null)
        {
            switch (value.Get<float>())
            {
                case 0:
                    if (CurrentSkull.CurrentEquip.Skill1.Action(_playerAim, actionSpawnPoint.position, _rotZ - 90))
                    {
                        //playerAnimator.SetTrigger(Constants.AniParams.Action);
                    }

                    break;

                case 1:
                    if (CurrentSkull.CurrentEquip.Skill2.Action(_playerAim, actionSpawnPoint.position, _rotZ - 90))
                    {
                        //playerAnimator.SetTrigger(Constants.AniParams.Action);
                    }

                    break;
            }
        }
    }

    private void OnStatus(InputValue value)
    {
        if (Managers.UI.IsPopupUI())
        {
            if (_uiMastery != null)
            {
                Managers.UI.ClosePopupUI(_uiMastery);
            }
        }
        else
        {
            _moveInput = Vector2.zero;

            _uiMastery = CurrentSkull?.MasteryController.ShowMasteryPopup();
        }
    }

    private void OnESC(InputValue value)
    {
        if (Managers.UI.FindPopup<UI_Tutorial>())
            return;

        if (!Managers.UI.CloseAllPopupUI())
        {
            Managers.Sound.PlaySound(Data.SoundType.Interaction);
            Managers.UI.ShowPopupUI<UI_Settings>();
            return;
        }
    }

    private void OnMap(InputValue value)
    {
        _minimapCam.ToggleExpand();
    }

    public void SetMinimapCam(MinimapCameraController _cam) => _minimapCam = _cam;

    #endregion

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponentInChildren(out NewOutline obj))
        {
            if (CurrentSkull == null && obj.Type == InteractType.Equip)
            {
                if (_collideEquip != null)
                {
                    _collideEquip = null;
                    obj.ShowOutline(false);
                }

                return;
            }

            obj.ShowOutline(true);

            if (collision.TryGetComponent(out InteractNPC npc))
            {
                _collideNPC = npc;
            }
            else if (collision.TryGetComponent(out SkullController skull))
            {
                _collideSkull = skull;
            }
            else if (collision.TryGetComponent(out NewEquip equip))
            {
                _collideEquip = equip;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponentInChildren(out NewOutline obj))
        {
            obj.ShowOutline(false);

            if (collision.TryGetComponent(out InteractNPC _))
            {
                _collideNPC = null;
            }
            else if (collision.TryGetComponent(out SkullController _))
            {
                _collideSkull = null;
            }
            else if (collision.TryGetComponent(out NewEquip _))
            {
                _collideEquip = null;
            }
        }
    }

    private void MouseUIControl()
    {
        // 마우스 커서 제작 시 적용
        //Vector2 worldPoint = Input.mousePosition;
        //RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
    }

    private void SetSkull(SkullController skull)
    {
        CurrentSkull = skull;
        CurrentSkull.SetMaster();

        playerAnimator.runtimeAnimatorController = skull.originAnimator;

        if (CurrentSkull.CurrentEquip != null)
        {
            equipRenderer.sprite = CurrentSkull.CurrentEquip.ArmedSprite;
            _ui.ActiveCooltimeUI(CurrentSkull.CurrentEquip);
        }

        _ui.ActiveMasteryButton(CurrentSkull);

        playerAnimator.SetTrigger(Constants.AniParams.Possess);

        //=========Aura=======//
        auraRenderer.gameObject.SetActive(CurrentSkull.AuraSystem.IsAuraOn);

        //========UI=========//
        //TODO 다른 캡슐화 방법이 있으련지 ??
        SlaveMasteryController mastery = CurrentSkull.MasteryController;
        _unitMastery.SetMasteryArray(mastery.MainMastery, mastery.SubMastery);

        uiUnitStatus.SetBuffSystem(CurrentSkull.BuffSystem);
        uiUnitStatus.healthSystem = CurrentSkull.Health;
        uiUnitStatus.gameObject.SetActive(true);

        SubscribeEvents();
        Managers.Sound.PlaySound(Data.SoundType.Possess);
    }

    private void ReleaseSkull()
    {
        CurrentSkull.ReleaseMaster(transform.position);
        UnsubscribeEvents();
        CurrentSkull = null;

        //=========Aura=======//
        auraRenderer.gameObject.SetActive(false);

        //=======UI========//
        //todo 다른 캡슐화 방법 있는지?
        uiUnitStatus.gameObject.SetActive(false);
        uiUnitStatus.healthSystem = null;
        uiUnitStatus.buffSystem = null;
        _unitMastery.SetMasteryArray();

        playerAnimator.runtimeAnimatorController = originAnimator;
        equipRenderer.sprite = null;

        _ui.UnactiveCooltimeUI();
        _ui.UnactiveMasteryButton();
    }

    private void SetEquip(NewEquip equip)
    {
        equip.Equipped(CurrentSkull);
        CurrentSkull.SetEquip(equip);
        equipRenderer.sprite = equip.ArmedSprite;

        _ui.ActiveCooltimeUI(CurrentSkull.CurrentEquip);

        Managers.Sound.PlaySound(Data.SoundType.Equip);
    }

    private void ReleaseEquip()
    {
        CurrentSkull.ReleaseEquip(transform.position);
        equipRenderer.sprite = null;

        _ui.UnactiveCooltimeUI();
    }

    private void SubscribeEvents()
    {
        CurrentSkull.AuraSystem.AuraOnEvent += ShowAuraEffect;
        CurrentSkull.MasteryController.OnMainMasteryOpened += ShowMasteryIcons;
        CurrentSkull.MasteryController.OnSubMasteryOpened += ShowMasteryIcons;
    }

    private void UnsubscribeEvents()
    {
        CurrentSkull.AuraSystem.AuraOnEvent -= ShowAuraEffect;
        CurrentSkull.MasteryController.OnMainMasteryOpened -= ShowMasteryIcons;
        CurrentSkull.MasteryController.OnSubMasteryOpened -= ShowMasteryIcons;
    }

    private void ShowAuraEffect()
    {
        auraRenderer.gameObject.SetActive(true);
    }

    private void ShowMasteryIcons(Mastery _)
    {
        SlaveMasteryController masteryController = CurrentSkull.MasteryController;
        if (masteryController == null) { return; }

        _unitMastery.SetMasteryArray(masteryController.MainMastery, masteryController.SubMastery);
    }
}