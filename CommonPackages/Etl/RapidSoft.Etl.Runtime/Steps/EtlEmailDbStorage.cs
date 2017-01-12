namespace RapidSoft.Etl.Runtime.Steps
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [Serializable]
    [TypeConverter(typeof(EtlEmailDbStorageObjectConverter))]
    public sealed class EtlEmailDbStorage
    {
        public string ConnectionString { get; set; }

        public string SchemaName { get; set; }
        
        #region Nested classes

        public sealed class EtlEmailDbStorageObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlEmailDbStorage)value;

                    if (obj != null)
                    {
                        return obj.ConnectionString;
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

                return new EtlEmailDbStorage { ConnectionString = str };
            }
        }

        #endregion

        public void ThrowIfInvalid()
        {
            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentException("Property \"ConnectionString\" cannot be null or empty");
            }

            if (string.IsNullOrEmpty(this.SchemaName))
            {
                throw new ArgumentException("Property \"SchemaName\" cannot be null or empty");
            }
        }
    }
}