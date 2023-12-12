using System;

namespace Data.Remote.Response
{
    [Serializable]
    public class SettingResponseBody
    {
        public string Property;
        public string DataType;
        public float Value;
    }
}