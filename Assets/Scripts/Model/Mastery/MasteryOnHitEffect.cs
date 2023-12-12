using System;
using Util;

public class MasteryOnHitEffect : MasteryEffect
{
    public OnHit.OnHitType OnHitType { get; private set; }

    public MasteryOnHitEffect()
    {
    }

    public MasteryOnHitEffect(
        OnHit.OnHitType onHitType,
        Mastery.MasteryEffectType type) : base(type)
    {
        OnHitType = onHitType;
    }

    public override string StringRes()
    {
        return OnHitType switch
        {
            OnHit.OnHitType.LifeStealOnHit => Constants.StringRes.EffectLifeStealId,
            OnHit.OnHitType.RageOnHit => Constants.StringRes.EffectRageId,
            OnHit.OnHitType.ChainLightningOnHit => Constants.StringRes.EffectChainLightningId,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override string ToFormatString(string format)
    {
        return format;
    }
}