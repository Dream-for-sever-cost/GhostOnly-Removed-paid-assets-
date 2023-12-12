using System;

public class BuffModel
{
    public enum BuffType
    {
        None,
        SlowMovementSpdHalf = 9,
        NormalActionSpeed = 13,
        NormalMovementSpeed = 18,
        NormalCriticalBuff,
        SlowMovementSpd,
        NormalMeleeDamageBuff,
        NormalRangeDamageBuff,
        NormalMagicDamage,
        RageBuff = 27,
        SlowDeBuff,
        HorrorDeBuff,
    }

    public BuffType Type;
    public StatType[] StatTypes;
    public float LastingTime;
    public float AddedValue;
    public float MultipleValue;

    public BuffModel()
    {
    }

    public BuffModel(StatType[] statTypes, float lastingTime, float addedValue, BuffType type)
    {
        StatTypes = statTypes;
        LastingTime = lastingTime;
        AddedValue = addedValue;
        Type = type;
    }

    public BuffModel(StatType[] statTypes, float lastingTime, float addedValue, float multipleValue, BuffType type)
    {
        StatTypes = statTypes;
        LastingTime = lastingTime;
        AddedValue = addedValue;
        MultipleValue = multipleValue;
        Type = type;
    }

    public static BuffModel None =
        new BuffModel(Array.Empty<StatType>(), lastingTime: 0f, addedValue: 0f, BuffType.None);

    public static BuffType StatTypeToBuffType(StatType statType)
    {
        return statType switch
        {
            StatType.MaxHP => BuffType.None,
            StatType.MoveSpeed => BuffType.NormalMovementSpeed,
            StatType.Strength => BuffType.NormalMeleeDamageBuff,
            StatType.Dexterity => BuffType.NormalRangeDamageBuff,
            StatType.Intelligence => BuffType.NormalMagicDamage,
            StatType.CriticalRate => BuffType.NormalCriticalBuff,
            StatType.ActionDelay => BuffType.NormalActionSpeed,
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
    }
}