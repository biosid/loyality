using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace RapidSoft.Etl.Runtime.DataSources.Xml
{
    internal static class XmlNameComparer
    {
        public static bool IsEqualElementNames(string name0, string name1)
        {
            return (string.Equals(name0, name1, StringComparison.InvariantCulture));
        }
    }
}
