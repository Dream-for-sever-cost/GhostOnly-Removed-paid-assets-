using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

public sealed class ChainLightning : PoolAble
{
    [SerializeField] private TrailRenderer trailRenderer;
    [HideInInspector] public ObjectPoolManager poolManager;

    [SerializeField] private PoolType vfxPoolType;

    //ChainLightning States
    private bool _isReady = false;
    private HashSet<Transform> _damagedTargets;
    private Transform _nextTarget;
    private LayerMask _target;
    private Vector2 _direction;
    private float _damage;
    private int _chainCount;
    private int _curChainCount;
    private float _maxDistance;

    public Action<PoolType> ShowVfxEvent;

    private void Awake()
    {
        poolManager = ObjectPoolManager.Instance;
        _damagedTargets = new HashSet<Transform>();
    }

    private void OnDisable()
    {
        _isReady = false;
        _damagedTargets.Clear();
    }

    public void Initialize(
        LayerMask target,
        Vector2 position,
        float damage,
        float maxDistance,
        int chainCount)
    {
        transform.position = position;
        _target = target;
        _damage = damage;
        _chainCount = chainCount;
        _maxDistance = maxDistance;
        _curChainCount = 0;
        _nextTarget = FindNextTarget(maxDistance, transform);
        trailRenderer.Clear();
        _isReady = true;
    }

    private void Update()
    {
        if (!_isReady) { return; }

        if (_chainCount <= _curChainCount || _nextTarget == null)
        {
            _isReady = false;
            Invoke(nameof(ReleaseObject), 0.2f);
            return;
        }

        //이동 처리만 행하도록
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (!_nextTarget.gameObject.activeInHierarchy)
        {
            _nextTarget = FindNextTarget(_maxDistance, transform);
            return;
        }

        transform.position = _nextTarget.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //todo migrate to hit system 
        if (_target.value == (_target.value | 1 << other.gameObject.layer) && _chainCount > _curChainCount)
        {
            Transform damaged = other.transform;
            if (damaged.TryGetComponent(out IDamagable targetDamagable))
            {
                if (targetDamagable.IsDeath())
                {
                    _nextTarget = FindNextTarget(_maxDistance, transform);
                    return;
                }
                
                if (!_damagedTargets.Contains(damaged))
                {
                    _curChainCount++;
                    targetDamagable.TakeDamage(_damage);
                    _damagedTargets.Add(damaged);

                    //=====Chain Lightning Effect ======//
                    ShowVfxEvent?.Invoke(vfxPoolType);
                    GameObject effect = poolManager.GetGo(vfxPoolType);
                    effect.transform.SetParent(damaged, false);
                    
                    _nextTarget = FindNextTarget(_maxDistance, transform);
                }
            }
        }
    }

    private Transform FindNextTarget(float maxDistance, Transform chainTrans)
    {
        Transform target =
            Managers.Target.GetNearestTargetWithinDistance(chainTrans, _target, maxDistance, _damagedTargets);

        return target;
    }
}