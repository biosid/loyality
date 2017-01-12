using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime
{
    [Serializable]
    public enum EtlVariableBinding
    {
        None,

        [XmlEnum("Value")]
        Obsolete_Value,

        [XmlEnum("String")]
        Obsolete_String,

        EtlPackageId,
        EtlSessionId,
        ParentEtlSessionId,
        UserName,
        
        EtlSessionDate,
        EtlSessionDateTime,
        EtlSessionYear,
        EtlSessionYear4,
        EtlSessionMonth,
        EtlSessionMonth2,
        EtlSessionDay,
        EtlSessionDay2,
        EtlSessionHour,
        EtlSessionHour2,
        EtlSessionMinute,
        EtlSessionMinute2,
        EtlSessionSecond,
        EtlSessionSecond2,

        EtlSessionUtcDate,
        EtlSessionUtcDateTime,
        EtlSessionUtcYear,
        EtlSessionUtcYear4,
        EtlSessionUtcMonth,
        EtlSessionUtcMonth2,
        EtlSessionUtcDay,
        EtlSessionUtcDay2,
        EtlSessionUtcHour,
        EtlSessionUtcHour2,
        EtlSessionUtcMinute,
        EtlSessionUtcMinute2,
        EtlSessionUtcSecond,
        EtlSessionUtcSecond2,

        TAB,
        CR,
        LF,
        //NewLine,
        EmptyString,
    }
}
