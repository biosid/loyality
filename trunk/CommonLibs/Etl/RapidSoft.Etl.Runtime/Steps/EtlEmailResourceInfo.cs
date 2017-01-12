using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [TypeConverter(typeof(EtlResourceInfoObjectConverter))]
    public sealed class EtlEmailResourceInfo : EtlDataSourceInfo
    {
        #region Properties

        public string HostName
        {
            get;
            set;
        }

        public int Port
        {
            get; 
            set;
        }

        public EtlResourceCredential Credential
        {
            get;
            set;
        }

        public bool UseSSL
        {
            get;
            set;
        }

        public List<EtlReceiveMailFilter> Filters
        {
            get; 
            set;
        }

        public EtlTimeSpan timeSpan
        {
            get; 
            set;
        }

        public string AttachmentRegExp
        {
            get; 
            set;
        }

        public string DbConnectionString { get; set; }

        public string DbSchemaName { get; set; }

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
                    var obj = (EtlEmailResourceInfo)value;

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

                return new EtlEmailResourceInfo
                {
                    Name = str,
                };
            }
        }

        #endregion
    }
}
