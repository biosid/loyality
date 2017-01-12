using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [TypeConverter(typeof(EtlResourceInfoObjectConverter))]
    public sealed class EtlResourceInfo : EtlDataSourceInfo
    {
        #region Properties

        public string Uri
        {
            get;
            set;
        }

        public EtlResourceCredential Credential
        {
            get;
            set;
        }

        public bool AllowInvalidCertificates
        {
            get;
            set;
        }

        public string Method
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        [XmlArrayItem("Header")]
        public List<EtlRequestHeader> Headers
        {
            [DebuggerStepThrough]
            get
            {
                return _headers;
            }
        }
        private readonly List<EtlRequestHeader> _headers = new List<EtlRequestHeader>();

        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=\"2.0.0.0\", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=\"2.0.0.0\", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string Request
        {
            get;
            set;
        }

        #endregion

        #region Nested classes

        public sealed class EtlResourceInfoObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlResourceInfo)value;

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

                return new EtlResourceInfo
                {
                    Name = str,
                };
            }
        }

        #endregion
    }
}
