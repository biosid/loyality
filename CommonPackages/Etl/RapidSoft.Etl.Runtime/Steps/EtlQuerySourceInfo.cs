using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [TypeConverter(typeof(EtlQuerySourceInfoObjectConverter))]
    public sealed class EtlQuerySourceInfo : EtlDataSourceInfo
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

        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=\"2.0.0.0\", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=\"2.0.0.0\", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string Text
        {
            get;
            set;
        }

        public List<EtlQueryParameter> Parameters
        {
            [DebuggerStepThrough]
            get
            {
                return _parameters;
            }
        }
        private List<EtlQueryParameter> _parameters = new List<EtlQueryParameter>();

        #endregion

        #region Nested classes

        public sealed class EtlQuerySourceInfoObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlQuerySourceInfo)value;

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

                return new EtlQuerySourceInfo
                {
                    Name = str,
                };
            }
        }

        #endregion
    }
}
