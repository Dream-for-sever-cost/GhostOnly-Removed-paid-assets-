public sealed class MasteryAddStatEffect : MasteryEffect
{
    public StatType StatType;
    public float AddedValue;
    public float PercentageValue;

    public MasteryAddStatEffect(
        Mastery.MasteryEffectType type,
        StatType statType,
        float addedValue,
        float percentageValue) : base(type)
    {
        StatType = statType;
        AddedValue = addedValue;
        PercentageValue = percentageValue;
    }
    public override string ToFormatString(string format)
    {
        if (AddedValue < 0)
        {
            format = format.Replace("+", string.Empty);
        }

        switch (StatType)
        {
            case StatType.MaxHP:
            case StatType.Strength:
            case StatType.Dexterity:
            case StatType.Intelligence:
            case StatType.CriticalRate:
            case StatType.Attack:
            default:
                return format.Replace("{AddedValue}", AddedValue.ToString("N0"));

            case StatType.MoveSpeed:
            case StatType.ActionDelay:
                return format.Replace("{AddedValue}", AddedValue.ToString("N1"));
        }
    }

    public override string StringRes()
    {
        return StatType.ToStatHeaderId();
    }
}