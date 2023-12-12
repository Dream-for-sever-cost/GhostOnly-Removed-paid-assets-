public class MasteryEffect
{
    public Mastery.MasteryEffectType Type;

    public MasteryEffect()
    {
    }

    public MasteryEffect(Mastery.MasteryEffectType type)
    {
        Type = type;
    }
    
    public virtual string StringRes()
    {
        return "";
    }

    public virtual string ToFormatString(string format)
    {
        return "";
    }
}