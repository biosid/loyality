using System;
using System.ComponentModel;
using System.Globalization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [TypeConverter(typeof(EtlCsvFileInfoObjectConverter))]
    public sealed class EtlCsvFileInfo : EtlFileInfo
    {
        #region Properties

        public bool HasHeaders { get; set; }

        public string LineDelimiter { get; set; }

        public string FieldDelimiter { get; set; }

        public string Quote { get; set; }

        public string Escape { get; set; }

        public bool SkipEmptyRows { get; set; }

        public FileCounterDbStorage FileCounterDbStorage { get; set; }

        public bool NullForSkipColumn { get; set; }

        #endregion

        #region Nested classes

        public sealed class EtlCsvFileInfoObjectConverter : ExpandableObjectConverter
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

            public override object ConvertTo(
                ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    var obj = (EtlCsvFileInfo)value;

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

                return new EtlCsvFileInfo { Name = str, };
            }
        }

        #endregion
    }
}
