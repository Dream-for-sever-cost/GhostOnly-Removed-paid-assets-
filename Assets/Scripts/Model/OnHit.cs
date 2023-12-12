using System;

[Serializable]
public class OnHit
{
    public enum OnHitType
    {
        LifeStealOnHit,
        RageOnHit,
        ChainLightningOnHit,
    }

    public OnHitType Type;
    public StatType StatType;
    public string PoolType;
    public float DamageCoefficient;
    public float Percent;
    public float LastingTime;
    public float EffectAmount;

    public OnHit()
    {
    }

    public OnHit(OnHitType type, string poolType, float percent, float effectAmount, float lastingTime,
        StatType statType, float damageCoefficient)
    {
        Type = type;
        PoolType = poolType;
        Percent = percent;
        EffectAmount = effectAmount;
        LastingTime = lastingTime;
        StatType = statType;
        DamageCoefficient = damageCoefficient;
    }
}