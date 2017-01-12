using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml;
using System.Data;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [DebuggerDisplay("{CounterName}")]
    [TypeConverter(typeof(EtlCounterBindingObjectConverter))]
    public sealed class EtlCounterBinding
    {
        #region Constructors

        public EtlCounterBinding()
        {
        }

        public EtlCounterBinding(string entityName, string counterName)
        {
            this.EntityName = entityName;
            this.CounterName = counterName;
        }

        #endregion

        #region Properties

        public string EntityName
        {
            get;
            set;
        }

        public string CounterName
        {
            get;
            set;
        }

        #endregion

        #region Nested classes

        public sealed class EtlCounterBindingObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlCounterBinding)value;

                    if (obj != null)
                    {
                        return obj.CounterName;
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
                if (str == "")
                {
                    return null;
                }

                return new EtlCounterBinding(null, str);
            }
        }

        #endregion
    }
}
