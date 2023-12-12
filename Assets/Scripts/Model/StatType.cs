using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum StatType
{
    MaxHP,
    MoveSpeed,
    Strength,
    Dexterity,
    Intelligence,
    CriticalRate,
    ActionDelay,
    Attack,
}

[System.Serializable]
public class Condition
{
    public float Value => Mathf.Clamp((BaseValue + AddedValue) * _multipliedValue, MinValue, MaxValue);
    public float BaseValue { get; private set; }
    public float AddedValue { get; private set; }

    public float MinValue { get; private set; }
    public float MaxValue { get; private set; }
    public bool IsMax => (BaseValue >= MaxValue);
    private float _multipliedValue = 1f;

    public Condition(float cur, float max)
    {
        BaseValue = cur;
        MinValue = 0f;
        MaxValue = max;
    }

    public Condition(float cur, float max, float minValue)
    {
        BaseValue = cur;
        MinValue = minValue;
        MaxValue = max;
    }

    public Condition(float cur)
    {
        BaseValue = cur;
        MinValue = 0f;
        MaxValue = cur;
    }

    public void SetValue(float value)
    {
        BaseValue = Mathf.Clamp(value, MinValue, MaxValue);
    }

    public void ClearAddedValue()
    {
        AddedValue = 0f;
    }

    public void AddValue(float value)
    {
        AddedValue += value;
    }

    public void MultipleValue(float value)
    {
        _multipliedValue += value;
    }
}