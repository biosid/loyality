using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [TypeConverter(typeof(EtlMethodSourceInfoObjectConverter))]
    public sealed class EtlMethodSourceInfo : EtlDataSourceInfo
    {
        #region Properties

        public string AssemblyName
        {
            get;
            set;
        }

        public string TypeName
        {
            get;
            set;
        }

        public string MethodName
        {
            get;
            set;
        }

        public List<EtlMethodParameter> Parameters
        {
            [DebuggerStepThrough]
            get
            {
                return _parameters;
            }
        }
        private List<EtlMethodParameter> _parameters = new List<EtlMethodParameter>();

        #endregion

        #region Nested classes

        public sealed class EtlMethodSourceInfoObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlMethodSourceInfo)value;

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

                return new EtlMethodSourceInfo
                {
                    Name = str,
                };
            }
        }

        #endregion
    }
}
