using System;
using System.Data;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime
{
    [Serializable]
    [TypeConverter(typeof(EtlValueFunctionObjectConverter))]
    public abstract class EtlValueFunction
    {
        #region Constructors

        public EtlValueFunction()
        {
        }

        #endregion

        #region Methods

        public abstract object Evaluate(object value, IDataRecord sourceRecord);

        public override string ToString()
        {
            return this.GetType().Name;
        }

        #endregion

        #region Nested classes

        public sealed class EtlValueFunctionObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlValueFunction)value;

                    if (obj != null)
                    {
                        return obj.GetType().ToString();
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

                var type = Type.GetType(str);
                return Activator.CreateInstance(type);
            }
        }

        #endregion
    }
}
