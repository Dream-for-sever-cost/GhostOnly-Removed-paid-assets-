using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager
{
    public enum TargetType
    {
        Slave = 1,
        Alter = 10,
    }

    private const int MaxTarget = 16;
    private Transform[] _transforms = new Transform[MaxTarget];
    private readonly Dictionary<Transform, int> _slaves = new Dictionary<Transform, int>();
    private readonly List<Transform> _enemies = new List<Transform>();

    public void Init()
    {
        _transforms.Initialize();
        _slaves.Clear();
        _enemies.Clear();
    }

    public void AddSlave(Transform tf, TargetType type)
    {
        if (_slaves.ContainsKey(tf))
            return;

        _slaves.Add(tf, (int)type);
    }

    public void RemoveSlave(Transform tf)
    {
        _slaves.Remove(tf);
    }

    public void AddEnemy(Transform tf)
    {
        if (_enemies.Contains(tf))
            return;

        _enemies.Add(tf);
    }

    public void RemoveEnemy(Transform tf)
    {
        _enemies.Remove(tf);
    }

    public Transform GetWeightedAggro(Transform agent)
    {
        if (_slaves.Count < 1)
            return agent;

        Transform returnTransform = null;
        float maxAggro = float.MinValue;
        float currentAggro;

        foreach ((Transform slave, int weight) in _slaves)
        {
            //todo using util function
            Vector2 dist = slave.position - agent.position;
            float magnitude = dist.x * dist.x + dist.y * dist.y;
            currentAggro = weight * (1 / magnitude);

            if (currentAggro > maxAggro)
            {
                maxAggro = currentAggro;
                returnTransform = slave;
            }
        }

        return returnTransform;
    }

    public Transform GetNearestTarget(Transform agent, LayerMask targetLayer)
    {
        return GetNearestTargetWithinDistance(agent, targetLayer, float.MaxValue, Array.Empty<Transform>());
    }

    public Transform GetNearestTargetWithinDistance(
        Transform agent,
        LayerMask targetLayer,
        float distance,
        IReadOnlyCollection<Transform> excludes)
    {
        if (_enemies.Count < 1)
            return null;

        Transform returnTransform = null;
        float minDist = float.MaxValue;


        foreach (Transform target in _enemies)
        {
            if (targetLayer.value != (targetLayer.value | 1 << target.gameObject.layer))
            {
                continue;
            }

            bool isExclude = false;
            foreach (Transform exclude in excludes)
            {
                if (target == exclude)
                {
                    isExclude = true;
                    break;
                }
            }

            if (isExclude) { continue; }

            float dist = (target.position - agent.position).magnitude;

            if (dist < minDist && dist < distance)
            {
                minDist = dist;
                returnTransform = target;
            }
        }

        return returnTransform;
    }

    public Transform[] GetTargets(LayerMask targetMask, Vector2 position, float dist, out int size)
    {
        //todo index out of boundaries ...
        for (int i = 0; i < MaxTarget; i++)
        {
            _transforms[i] = null;
        }

        dist *= dist;
        int curIndex = -1;
        //todo enemy + alliance
        for (int i = 0; i < _enemies.Count; i++)
        {
            //todo layerMask
            Vector2 targetPos = _enemies[i].position;
            Vector2 vector = targetPos - position;

            float distToTarget = vector.x * vector.x + vector.y * vector.y;

            if (dist >= distToTarget)
            {
                if (curIndex < MaxTarget-1)
                {
                    //todo fix bug
                    curIndex++;
                    _transforms[curIndex] = _enemies[i];
                }
            }
        }

        size = curIndex + 1;
        return _transforms;
    }
}