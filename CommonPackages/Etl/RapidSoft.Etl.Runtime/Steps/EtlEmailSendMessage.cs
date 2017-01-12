namespace RapidSoft.Etl.Runtime.Steps
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [Serializable]
    [TypeConverter(typeof(EtlEmailSendMessageObjectConverter))]
    public class EtlEmailSendMessage
    {
        #region Properties

        public string From { get; set; }

        public string[] To { get; set; }

        public string Subject { get; set; }

        public string AttachmentFileMask { get; set; }

        #endregion

        #region Nested classes

        public sealed class EtlEmailSendMessageObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlEmailSendMessage)value;

                    if (obj != null)
                    {
                        return obj.Subject;
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

                return new EtlEmailSendMessage { Subject = str };
            }
        }

        #endregion

        public void ThrowIfInvalid()
        {
            if (this.To == null || this.To.Length == 0)
            {
                throw new ArgumentException("Property \"To\" cannot be null or empty");
            }

            if (string.IsNullOrEmpty(this.From))
            {
                throw new ArgumentException("Property \"From\" cannot be null or empty");
            }
        }
    }
}