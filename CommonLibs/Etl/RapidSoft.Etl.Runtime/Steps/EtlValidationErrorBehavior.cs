using System;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    public enum EtlValidationErrorBehavior
    {
        ValidateUntilError,
        ValidateAll
    }
}
