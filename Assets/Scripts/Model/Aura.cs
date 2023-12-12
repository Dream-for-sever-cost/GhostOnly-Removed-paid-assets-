namespace Model
{
    public class Aura
    {
        public Aura()
        {
        }

        public Aura(
            StatType auraStatType,
            AuraType type,
            float auraRange,
            float effectBaseValue,
            bool isDeBuff,
            float effectCoefficient)
        {
            Type = type;
            AuraRange = auraRange;
            EffectBaseValue = effectBaseValue;
            IsDeBuff = isDeBuff;
            EffectCoefficient = effectCoefficient;
            AuraStatType = auraStatType;
        }

        public enum AuraType
        {
            SunFireAura,
            FrozenAura,
            CurseAura,
        }

        public StatType AuraStatType { get; private set; }
        public AuraType Type { get; private set; }
        public float AuraRange { get; private set; }
        public float EffectBaseValue { get; private set; }
        public float EffectCoefficient { get; private set; }
        public bool IsDeBuff { get; private set; }
    }
}