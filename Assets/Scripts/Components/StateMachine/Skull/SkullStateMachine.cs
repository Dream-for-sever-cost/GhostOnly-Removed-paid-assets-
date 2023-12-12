using Component.Slave;
using Manager;
using System;
using StateMachine.Slave;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using Util;

public sealed class SkullStateMachine : StateMachine<SkullBaseState.ESkullState>
{
    [SerializeField] private SlaveRangeController _rangeController;
    [field: SerializeField] public SlaveManager SlaveManager { get; private set; }
    [field: SerializeField] public SkullController SkullController { get; private set; }
    [field: SerializeField] public SpriteRenderer WeaponRenderer { get; private set; }
    [field: SerializeField] public SpriteRenderer SkullRenderer { get; private set; }
    [field: SerializeField] public Transform ActionPosition { get; private set; }
    [field: SerializeField] public Transform RotationPosition { get; private set; }
    [field: SerializeField] public Rigidbody2D RigidBody { get; private set; }

    public StatController StatController { get; private set; }


    public Dictionary<Transform, Collider2D> Walls { get; private set; } = new();
    public Animator Animator { get; private set; }
    public bool IsHit { get; set; }
    public GameObject Target { get; private set; }
    public HealthSystem HealthSystem { get; private set; }

    public float Speed => StatController.Stats[StatType.MoveSpeed].Value;
    public float RangeOfAction { get; private set; } = 3.0f;
    public float RangeOfFollow => RangeOfAction + followRange;
    public float followRange = Constants.Distance.FollowRange;
    public float ActionDelay { get; private set; } = 1f;

    public bool IsEquipped => SkullController.CurrentEquip != null;

    public LayerMask TargetMask => SkullController.CurrentEquip == null
        ? LayerMask.NameToLayer("Default")
        : SkullController.CurrentEquip.Action.Target;

    public bool isLamper = false;

    private bool _isControlling = false;

    private float _timeSinceLastDamaged = 0f;
    private float _damageTime = 0.5f;
    //===============================================================

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Death = Animator.StringToHash("Die");
    private static readonly int Action = Animator.StringToHash("Action");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private bool _isReady = false;


    private static float[] _deltaOffsetX = { 0.5f, 0.5f, -0.5f, -0.5f };
    private static float[] _deltaOffsetY = { 0.5f, -0.5f, 0.5f, -0.5f };

    protected override void Awake()
    {
        base.Awake();
        HealthSystem = GetComponent<HealthSystem>();
        RigidBody = GetComponent<Rigidbody2D>();
        SkullController = GetComponent<SkullController>();
        StatController = SkullController.Stat;

        if (!gameObject.TryGetComponentInChildren(out _rangeController))
        {
            Debug.LogAssertion("Cannot get RangeController in children");
        }
    }

    private void Start()
    {
        foreach (SkullBaseState.ESkullState state in Enum.GetValues(typeof(SkullBaseState.ESkullState)))
        {
            switch (state)
            {
                case SkullBaseState.ESkullState.Idle:
                    _states.Add(state, new SkullIdleState(this));
                    break;
                case SkullBaseState.ESkullState.Move:
                    _states.Add(state, new SkullMovingState(this));
                    break;
                case SkullBaseState.ESkullState.Action:
                    _states.Add(state, new SkullActionState(this));
                    break;
                case SkullBaseState.ESkullState.Death:
                    _states.Add(state, new SkullDeathState(this));
                    break;
                case SkullBaseState.ESkullState.Hit:
                    _states.Add(state, new SkullHitState(this));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        CurrentState = _states[SkullBaseState.ESkullState.Idle];
        Animator = GetComponentInChildren<Animator>();
        BaseState = _states[SkullBaseState.ESkullState.Idle];
        _rangeController.OnFindTargetEvent += SetTarget;
        SkullController.OnRecoveryEvent += RecoveryHealth;
        _rangeController.OnDisappearTargetEvent += RemoveTarget;
        Init(Managers.SlaveManager);
    }

    private void OnEnable()
    {
        HealthSystem.OnGettingDamageEvent += TakeDamage;
    }

    public void Init(SlaveManager manager)
    {
        if (_isReady) { return; }

        _isReady = true;
        SlaveManager = manager;
        HealthSystem.DecreaseHealth(HealthSystem.MaxHealth);
    }

    private void SetTarget(GameObject target)
    {
        Target = target;
    }

    private void RemoveTarget()
    {
        Target = null;
        followRange = Constants.Distance.FollowRange;
    }

    public void ReleaseSlave()
    {
        Walls.Clear();
        _isControlling = false;
        Target = null;
        Animator.SetTrigger(Idle);

        if (SkullController.CurrentEquip != null)
        {
            WeaponRenderer.sprite = SkullController.CurrentEquip.ArmedSprite;
            RangeOfAction = SkullController.CurrentEquip.Action.Range;
            ActionDelay = SkullController.CurrentEquip.Action.Cooltime;
        }

        if (SkullController.CurrentEquip)
        {
            isLamper = SkullController.CurrentEquip.Type == NewEquip.EquipType.Lamp;
        }

        CurrentState = BaseState;
        Managers.Target.AddSlave(transform, TargetManager.TargetType.Slave);
    }

    public void ControlSlave()
    {
        _isControlling = true;

        //todo make event
        Managers.Target.RemoveSlave(transform);
    }

    protected override void Update()
    {
        DecreaseHealth();
        base.Update();
    }

    private void RecoveryHealth()
    {
        float healAmount = Managers.GameManager.BaseHealAmount + Managers.GameManager.recoverySpeed;
        HealthSystem.TakeHeal(healAmount);
    }

    private void DecreaseHealth()
    {
        _timeSinceLastDamaged += Time.deltaTime;
        if (_timeSinceLastDamaged >= Managers.GameManager.TimePerDamage && !HealthSystem.IsDeath())
        {
            HealthSystem.DecreaseHealth(Managers.GameManager.BaseDecreaseAmount);
            _timeSinceLastDamaged = 0f;
        }
    }

    public void SetAnimationTrigger(SkullBaseState.ESkullState state)
    {
        if (state != SkullBaseState.ESkullState.Idle)
        {
            Animator.ResetTrigger(Idle);
            Animator.SetTrigger(AnimHashValue(state));
        }
    }

    public void ToggleAnimationBool(SkullBaseState.ESkullState state)
    {
        bool currentAnimValue = Animator.GetBool(AnimHashValue(state));
        Animator.SetBool(AnimHashValue(state), !currentAnimValue);
    }

    private int AnimHashValue(SkullBaseState.ESkullState state)
    {
        return state switch
        {
            SkullBaseState.ESkullState.Idle => Idle,
            SkullBaseState.ESkullState.Move => Move,
            SkullBaseState.ESkullState.Action => Action,
            SkullBaseState.ESkullState.Death => Death,
            SkullBaseState.ESkullState.Hit => Hit,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }

    private void TakeDamage(float damage)
    {
        IsHit = true;
    }

    public void SetDirection(Vector2 direction)
    {
        bool canMove = true;
        if (direction == Vector2.zero)
        {
            RigidBody.velocity = Vector2.zero;
            return;
        }

        Vector2 nextPosition = (Vector2)transform.position + (Speed * direction);
        foreach ((Transform trans, Collider2D wall) in Walls)
        {
            canMove = !wall.OverlapPoint(nextPosition);
            if (!canMove) { break; }
        }

        if (canMove)
        {
            RigidBody.velocity = direction * Speed;
        }
        else
        {
            RigidBody.velocity = Vector2.zero;
        }
    }

    public void Rotate(float deg)
    {
        RotationPosition.rotation = Quaternion.Euler(0, 0, deg + 180);
        SkullRenderer.flipX = Mathf.Abs(deg) < 90f;
        WeaponRenderer.flipY = Mathf.Abs(deg) < 90f;
    }
}