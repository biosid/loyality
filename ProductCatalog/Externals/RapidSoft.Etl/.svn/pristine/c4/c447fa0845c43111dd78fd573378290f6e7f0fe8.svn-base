namespace RapidSoft.Etl.Runtime.Steps
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Xml.Serialization;

    [Serializable]
    [TypeConverter(typeof(EtlEmailReceiveMessageObjectConverter))]
    public class EtlEmailReceiveMessage
    {
        [XmlArrayItem("ReceiveFilter")]
        public List<EtlEmailReceiveFilter> Filters
        {
            get;
            set;
        }

        public string AttachmentRegExp
        {
            get;
            set;
        }

        #region Nested classes

        public sealed class EtlEmailReceiveMessageObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlEmailReceiveMessage)value;

                    if (obj != null)
                    {
                        return obj.AttachmentRegExp;
                    }

                    return null;
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

                return new EtlEmailReceiveMessage { AttachmentRegExp = str };
            }
        }

        #endregion

        public void ThrowIfInvalid()
        {
            if (string.IsNullOrEmpty(this.AttachmentRegExp))
            {
                throw new ArgumentException("Property \"AttachmentRegExp\" cannot be null or empty");
            }
        }
    }
}