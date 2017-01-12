using System;

namespace RapidSoft.Etl.Logging
{
    [Serializable]
    public enum EtlVariableModifier
    {
        Input = 0,
        Output = 1,
        Bound = 2
    }
}
