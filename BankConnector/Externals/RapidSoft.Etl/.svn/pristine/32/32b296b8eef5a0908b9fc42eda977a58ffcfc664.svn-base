using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [DebuggerDisplay("{Name} = {Value}")]
    [TypeConverter(typeof(EtlQueryParameterObjectConverter))]
    public class EtlQueryParameter
    {
        #region Constructors

        public EtlQueryParameter()
        {
        }

        public EtlQueryParameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        #endregion

        #region Properties

        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public static IDictionary<string, object> ToDictionary(List<EtlQueryParameter> parameters)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var param in parameters)
            {
                dictionary.Add(param.Name, param.Value);
            }

            return dictionary;
        }

        #endregion

        #region Nested classes

        public sealed class EtlQueryParameterObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlQueryParameter)value;

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

                return new EtlQueryParameter
                {
                    Name = str
                };
            }
        }

        #endregion
    }
}
