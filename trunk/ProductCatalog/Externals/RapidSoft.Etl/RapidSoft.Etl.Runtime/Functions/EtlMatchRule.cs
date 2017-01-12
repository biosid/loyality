using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace RapidSoft.Etl.Runtime.Functions
{
    [Serializable]
    [DebuggerDisplay("{ToString()}")]
    [TypeConverter(typeof(EtlMatchRuleObjectConverter))]
    public sealed class EtlMatchRule
    {
        #region Constructors

        public EtlMatchRule()
        {
        }

        public EtlMatchRule(string regex, EtlMatchResult result)
        {
            this.Regex = regex;
            this.Result = result;
        }

        #endregion

        #region Properties

        public string Regex
        {
            get;
            set;
        }

        public bool CaseSensitive
        {
            get;
            set;
        }

        public EtlMatchResult Result
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
                this.Regex,
                " -> ",
                this.Result
            );
        }

        #endregion

        #region Nested classes

        public sealed class EtlMatchRuleObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlMatchRule)value;

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

                return new EtlMatchRule();
            }
        }

        #endregion
    }
}
