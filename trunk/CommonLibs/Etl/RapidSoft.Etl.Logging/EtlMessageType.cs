using System;

namespace RapidSoft.Etl.Logging
{
    [Serializable]
    public enum EtlMessageType
    {
        Unknown = 0,
        SessionStart = 1,
        SessionEnd = 2,
        StepStart = 3,
        StepEnd = 4,
        Error = 5,
        Warning = 6,
        Fail = 7,
        Information = 8,
        Debug = 9
    }
}
