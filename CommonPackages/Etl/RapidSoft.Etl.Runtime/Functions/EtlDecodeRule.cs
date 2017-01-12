using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Functions
{
    [Serializable]
    [DebuggerDisplay("{ToString()}")]
    [TypeConverter(typeof(EtlDecodeRuleObjectConverter))]
    public sealed class EtlDecodeRule
    {
        #region Constructors

        public EtlDecodeRule()
        {
        }

        public EtlDecodeRule(string value, string result)
        {
            this.Value = value;
            this.Result = result;
        }

        #endregion

        #region Properties

        [XmlElement("OutputValue")]
        [Browsable(false)]
        public string Obsolote_OutputValue
        {
            get
            {
                return this.Result;
            }
            set
            {
                this.Result = value;
            }
        }

        public string Value
        {
            get;
            set;
        }

        public bool CaseSensitive
        {
            get;
            set;
        }

        public string Result
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Concat
            (
                this.Value,
                " -> ",
                this.Result
            );
        }

        #endregion

        #region Nested classes

        public sealed class EtlDecodeRuleObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlDecodeRule)value;

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

                return new EtlDecodeRule();
            }
        }

        #endregion
    }
}
