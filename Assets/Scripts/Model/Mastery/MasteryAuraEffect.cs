using Model;
using System;
using Util;

public class MasteryAuraEffect : MasteryEffect
{
    public Aura.AuraType AuraType { get; private set; }

    public MasteryAuraEffect()
    {
    }

    public MasteryAuraEffect(Aura.AuraType auraType, Mastery.MasteryEffectType type) : base(type)
    {
        AuraType = auraType;
    }

    public override string StringRes()
    {
        return AuraType switch
        {
            Aura.AuraType.SunFireAura => Constants.StringRes.EffectSunFireAuraId,
            Aura.AuraType.FrozenAura => Constants.StringRes.EffectFrozenAuraId,
            Aura.AuraType.CurseAura => Constants.StringRes.EffectCurseAuraId,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override string ToFormatString(string format)
    {
        return format;
    }
}