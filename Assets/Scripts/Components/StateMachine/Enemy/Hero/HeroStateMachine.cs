using System;
using UnityEngine;
using Util;

public class HeroStateMachine : StateMachine<HeroBaseState.EHeroState>, IEnemyInitializer
{
    [field: Header("Hero Component")]
    [field: SerializeField] public Animator Ani { get; private set; }
    [field: SerializeField] public AgentController Agent { get; private set; }
    [field: SerializeField] public StatController Stat { get; private set; }
    [field: SerializeField] public HealthSystem Health { get; private set; }
    [field: SerializeField] public Transform AttackSpawnPoint { get; private set; }
    [field: SerializeField] public PoolAble Pool { get; private set; }
    [SerializeField] private SpriteRenderer _heroRenderer;
    [SerializeField] private GameObject _heroCanvas;

    [field: Header("Attack")]
    [field: SerializeField] public EquipAction Attack { get; private set; }
    [SerializeField] private SpriteRenderer _equipRenderer;
    [SerializeField] private Transform _equipTransform;

    public Transform Target { get; set; }
    public bool IsMoving { get; set; } = false;
    public bool IsAttack { get; set; } = false;
    public bool IsHit { get; set; } = false;

    private void Start()
    {
        foreach (HeroBaseState.EHeroState state in Enum.GetValues(typeof(HeroBaseState.EHeroState)))
        {
            switch (state)
            {
                case HeroBaseState.EHeroState.Idle:
                    _states.Add(state, new HeroIdleState(this));
                    break;

                case HeroBaseState.EHeroState.Move:
                    _states.Add(state, new HeroMoveState(this));
                    break;

                case HeroBaseState.EHeroState.Attack:
                    _states.Add(state, new HeroAttackState(this));
                    break;

                case HeroBaseState.EHeroState.Hit:
                    _states.Add(state, new HeroHitState(this));
                    break;

                case HeroBaseState.EHeroState.Death:
                    _states.Add(state, new HeroDeathState(this));
                    break;

                default:
                    break;
            }
        }

        CurrentState = _states[HeroBaseState.EHeroState.Idle];
    }

    private float recentSearchTime = 0f;

    protected override void FixedUpdate()
    {
        recentSearchTime -= Time.deltaTime;

        if (recentSearchTime < 0)
        {
            recentSearchTime = Constants.Hero.SearchCycle;
            Target = Managers.Target.GetWeightedAggro(transform);

            SetIsAttack();
            SetIsMoving();
        }

        base.FixedUpdate();
    }

    public void Initialize(Vector2 pos)
    {
        gameObject.transform.position = pos;

        if (_states.ContainsKey(HeroBaseState.EHeroState.Idle))
        {
            CurrentState = _states[HeroBaseState.EHeroState.Idle];
        }

        ChangeOnDieUI(true);

        Agent.Initialize(Attack.Range, Stat.Stats[StatType.MoveSpeed].Value);

        Attack.SetStat(Stat);

        Health.TakeHeal(Stat.Stats[StatType.MaxHP].Value);
        Health.OnGettingDamageEvent += (float _) => { IsHit = true; };

        Managers.Target.AddEnemy(transform);
    }

    public void RotateEquip(float rotZ)
    {
        _equipRenderer.flipY = Mathf.Abs(rotZ) < 90f;
        _equipTransform.rotation = Quaternion.Euler(0, 0, rotZ + 180);
    }

    public void FlipRenderer()
    {
        if (Agent.GetDirection())
        {
            _heroRenderer.flipX = true;
            RotateEquip(0);
        }
        else
        {
            _heroRenderer.flipX = false;
            RotateEquip(180);
        }
    }

    public void ChangeOnDieUI(bool alive)
    {
        _heroCanvas.SetActive(alive);
        _equipRenderer.enabled = alive;
    }

    public void SetIsMoving()
    {
        if (Target == null)
        {
            IsMoving = false;
        }
        else
        {
            IsMoving = (Target.position - transform.position).magnitude > Attack.Range;
        }
    }

    public void SetIsAttack()
    {
        if (Target == null)
        {
            IsAttack = false;
        }
        else if (!Attack.Able)
        {
            IsAttack = false;
        }
        else
        {
            IsAttack = Attack.Range > (Target.position - transform.position).magnitude;
        }
    }
}