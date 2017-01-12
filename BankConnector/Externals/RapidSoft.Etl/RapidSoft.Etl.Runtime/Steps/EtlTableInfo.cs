using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [TypeConverter(typeof(EtlTableSourceInfoObjectConverter))]
    public sealed class EtlTableInfo : EtlDataSourceInfo
    {
        #region Properties

        public string ConnectionString
        {
            get;
            set;
        }

        public string ProviderName
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        #endregion

        #region Nested classes

        public sealed class EtlTableSourceInfoObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlTableInfo)value;

                    if (obj != null)
                    {
                        return obj.Name;
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

                return new EtlTableInfo
                {
                    Name = str,
                };
            }
        }

        #endregion
    }
}
