using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [TypeConverter(typeof(EtlExecuteQueryCountersObjectConverter))]
    public class EtlExecuteQueryCounters
    {
        #region Properties

        public EtlCounterBinding RowCount
        {
            get;
            set;
        }

        #endregion

        #region Nested classes

        public sealed class EtlExecuteQueryCountersObjectConverter : ExpandableObjectConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                {
                    return true;
                }

                return false;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    return true;
                }

                return false;
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    var obj = (EtlExecuteQueryCounters)value;

                    if (obj != null)
                    {
                        return "Counters";
                    }
                    else
                    {
                        return null;
                    }
                }

                return null;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == null)
                {
                    return null;
                }

                var str = value.ToString();
                if (string.IsNullOrEmpty(str))
                {
                    return null;
                }

                return new EtlExecuteQueryCounters();
            }
        }

        #endregion
    }
}
