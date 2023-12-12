using System;
using UnityEngine;
using Util;

public sealed class BossStateMachine : StateMachine<BossBaseState.EBossState>, IEnemyInitializer
{
    [field: Header("Boss Component")]
    [field: SerializeField] public Animator Ani { get; private set; }
    [field: SerializeField] public AgentController Agent { get; private set; }
    [field: SerializeField] public StatController Stat { get; private set; }
    [field: SerializeField] public HealthSystem Health { get; private set; }
    [field: SerializeField] public Transform AttackSpawnPoint { get; private set; }
    [field: SerializeField] public PoolAble Pool { get; private set; }
    [SerializeField] private SpriteRenderer _bossRenderer;
    [SerializeField] private GameObject _bossCanvas;

    [field: Header("Attack")]
    [field: SerializeField] public EquipAction Attack { get; private set; }
    [SerializeField] private SpriteRenderer _equipRenderer;
    [SerializeField] private Transform _equipTransform;

    [field: Header("Skill")]
    [field: SerializeField] public EquipAction Skill { get; private set; }

    public Transform Target { get; set; }
    public bool IsMoving { get; set; }
    public bool IsAttack { get; set; }
    public bool IsSkill { get; set; }
    public bool IsHit { get; set; }

    private void Start()
    {
        foreach (BossBaseState.EBossState state in Enum.GetValues(typeof(BossBaseState.EBossState)))
        {
            switch (state)
            {
                case BossBaseState.EBossState.Idle:
                    _states.Add(state, new BossIdleState(this));
                    break;

                case BossBaseState.EBossState.Move:
                    _states.Add(state, new BossMoveState(this));
                    break;

                case BossBaseState.EBossState.Attack:
                    _states.Add(state, new BossAttackState(this));
                    break;

                case BossBaseState.EBossState.Concentrate:
                    _states.Add(state, new BossConcentrateState(this));
                    break;

                case BossBaseState.EBossState.Skill:
                    _states.Add(state, new BossSkillState(this));
                    break;

                case BossBaseState.EBossState.Hit:
                    _states.Add(state, new BossHitState(this));
                    break;

                case BossBaseState.EBossState.Death:
                    _states.Add(state, new BossDeathState(this));
                    break;

                default:
                    break;
            }
        }

        CurrentState = _states[BossBaseState.EBossState.Idle];
    }

    private float recentSearchTime = 0f;

    protected override void FixedUpdate()
    {
        recentSearchTime -= Time.deltaTime;

        if (recentSearchTime < 0)
        {
            recentSearchTime = Constants.Hero.SearchCycle;
            Target = Managers.Target.GetWeightedAggro(transform);

            IsAttack = SetIsAction(Attack);
            IsSkill = SetIsAction(Skill);
            SetIsMoving();
        }

        base.FixedUpdate();
    }

    public void Initialize(Vector2 pos)
    {
        gameObject.transform.position = pos;

        if (_states.ContainsKey(BossBaseState.EBossState.Idle))
        {
            CurrentState = _states[BossBaseState.EBossState.Idle];
        }

        ChangeOnDieUI(true);

        Agent.Initialize(Attack.Range, Stat.Stats[StatType.MoveSpeed].Value);

        Attack.SetStat(Stat);
        Skill.SetStat(Stat);

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
            _bossRenderer.flipX = true;
            RotateEquip(0);
        }
        else
        {
            _bossRenderer.flipX = false;
            RotateEquip(180);
        }
    }

    public void ChangeOnDieUI(bool alive)
    {
        _bossCanvas.SetActive(alive);
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

    public bool SetIsAction(EquipAction action)
    {
        if (Target == null)
        {
            return false;
        }
        else if (!action.Able)
        {
            return false;
        }
        else
        {
            return action.Range > (Target.position - transform.position).magnitude;
        }
    }

    //public void SetIsAttack()
    //{
    //    if (Target == null)
    //    {
    //        IsAttack = false;
    //    }
    //    else if (!Attack.Able)
    //    {
    //        IsAttack = false;
    //    }
    //    else
    //    {
    //        IsAttack = Attack.Range > (Target.position - transform.position).magnitude;
    //    }
    //}

    //public void SetIsSkill()
    //{
    //    if (Target == null)
    //    {
    //        IsSkill = false;
    //    }
    //    else if (!Skill.Able)
    //    {
    //        IsSkill = false;
    //    }
    //    else
    //    {
    //        IsSkill = Skill.Range > (Target.position - transform.position).magnitude;
    //    }
    //}
}