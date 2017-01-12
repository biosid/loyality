using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [TypeConverter(typeof(EtlExecuteProcedureOutputVariablesObjectConverter))]
    public class EtlExecuteProcedureOutputVariableSet
    {
        #region Properties

        [XmlArrayItem("FieldToVariable")]
        public List<EtlFieldToVariableAssignment> FirstRow
        {
            [DebuggerStepThrough]
            get
            {
                return _firstRow;
            }
        }
        private readonly List<EtlFieldToVariableAssignment> _firstRow = new List<EtlFieldToVariableAssignment>();

        #endregion

        #region Nested classes

        public sealed class EtlExecuteProcedureOutputVariablesObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlExecuteProcedureOutputVariableSet)value;

                    if (obj != null)
                    {
                        return "OutputVariables";
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

                return new EtlExecuteProcedureOutputVariableSet();
            }
        }

        #endregion
    }
}
