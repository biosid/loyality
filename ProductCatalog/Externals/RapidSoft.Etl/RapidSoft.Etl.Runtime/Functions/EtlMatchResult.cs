using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace RapidSoft.Etl.Runtime.Functions
{
    [Serializable]
    [DebuggerDisplay("{ToString()}")]
    [TypeConverter(typeof(EtlMatchResultObjectConverter))]
    public sealed class EtlMatchResult
    {
        #region Constructors

        public EtlMatchResult()
        {
        }

        public EtlMatchResult(string sourceFieldName, EtlValueTranslation valueTranslation, string defaultValue)
        {
            this.SourceFieldName = sourceFieldName;
            this.SourceFieldTranslation = valueTranslation;
            this.DefaultValue = defaultValue;
        }

        #endregion

        #region Properties

        public string SourceFieldName
        {
            get;
            set;
        }

        public EtlValueTranslation SourceFieldTranslation
        {
            get;
            set;
        }

        public string DefaultValue
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.IsNullOrEmpty(SourceFieldName) ? this.DefaultValue : this.SourceFieldName;
        }

        #endregion

        #region Nested classes

        public sealed class EtlMatchResultObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlMatchResult)value;

                    if (obj != null)
                    {
                        return obj.ToString();
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

                return new EtlMatchResult();
            }
        }

        #endregion
    }
}
