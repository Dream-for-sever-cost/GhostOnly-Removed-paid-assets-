using System.Collections.Generic;

public class Mastery
{
    //todo modeling
    public enum MasteryGrade
    {
        Normal,
        Rare,
        Epic,
        Legend
    }

    public enum MasteryType
    {
        Strength,
        Dexterity,
        Intelligence
    }

    public enum MasteryId
    {
        Intensive,
        Dangerous,
        Vigorous,
        SimpleHonesty,
        Fat,
        Agile,
        Nimble,
        Swift,
        Urgent,
        Thrilling,
        Smart,
        Heartless,
        Calculative,
        Timid,
        Stupid,
        Powerful,
        Spartan,
        Calm,
        Scared,
        Talented,
        Aged,
        LivingLegend,
        Brilliant,
        Dreamy,
        UnChained,
        Horror,
        Abyssal,
    }

    public enum MasteryEffectType
    {
        None,
        AddStatEffect,
        OnHitEffect,
        AuraEffect
    }

    public readonly string Name;
    public MasteryId IconName;
    public MasteryGrade Grade;
    public MasteryType Type;
    public List<MasteryEffect> Effects;

    public static readonly Mastery None = new Mastery(
        MasteryGrade.Normal,
        type: MasteryType.Strength,
        "");

    public Mastery(
        MasteryGrade grade,
        MasteryType type,
        string name,
        MasteryId iconName = MasteryId.Intensive)
    {
        Grade = grade;
        Type = type;
        Name = name;
        Effects = new List<MasteryEffect>();
        IconName = iconName;
    }

    public Mastery(
        MasteryGrade grade,
        MasteryType type,
        string name,
        List<MasteryEffect> effects,
        MasteryId iconName = MasteryId.Intensive)
    {
        Grade = grade;
        Type = type;
        Name = name;
        Effects = effects;
        IconName = iconName;
    }
}