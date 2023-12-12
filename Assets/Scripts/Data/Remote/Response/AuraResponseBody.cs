using Model;
using System;

namespace Data.Remote.Response
{
    [Serializable]
    public class AuraResponseBody
    {
        public Aura.AuraType Id;
        public StatType StatType;
        public float BaseValue;
        public float Coefficient;
        public bool IsDeBuff;
    }
}