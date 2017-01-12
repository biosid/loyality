using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml;
using System.Data;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [DebuggerDisplay("{ToDebuggerDisplayString()}")]
    [TypeConverter(typeof(EtlCounterToVariableAssignmentObjectConverter))]
    public sealed class EtlCounterToVariableAssignment
    {
        #region Constructors

        public EtlCounterToVariableAssignment()
        {
        }

        #endregion

        #region Properties

        [Category("1. Destination")]
        public string VariableName
        {
            get;
            set;
        }

        #endregion

        #region Methods

        private string ToDebuggerDisplayString()
        {
            return this.VariableName;
        }

        #endregion

        #region Nested classes

        public sealed class EtlCounterToVariableAssignmentObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlCounterToVariableAssignment)value;

                    if (obj != null)
                    {
                        return obj.ToDebuggerDisplayString();
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

                return new EtlCounterToVariableAssignment();
            }
        }

        #endregion
    }
}
