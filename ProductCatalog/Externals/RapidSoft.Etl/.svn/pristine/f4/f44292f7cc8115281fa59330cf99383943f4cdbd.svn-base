using System;
using System.ComponentModel;
using System.Globalization;

namespace RapidSoft.Etl.Runtime.Steps
{
    [Serializable]
    [TypeConverter(typeof(EtlCsvFileBatchInfoObjectConverter))]
    public sealed class EtlCsvFileBatchInfo : EtlFileInfo
    {
        #region Properties

        public bool HasHeaders
        {
            get;
            set;
        }

        public string LineDelimiter
        {
            get;
            set;
        }

        public string FieldDelimiter
        {
            get;
            set;
        }

        public string Quote
        {
            get;
            set;
        }

        public string Escape
        {
            get;
            set;
        }

	    public string FileMask
	    {
		    get; 
			set;
	    }

	    #endregion

        #region Nested classes

        public sealed class EtlCsvFileBatchInfoObjectConverter : ExpandableObjectConverter
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
					var obj = (EtlCsvFileBatchInfo)value;

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

				return new EtlCsvFileBatchInfo
                {
                    Name = str,
                };
            }
        }

        #endregion
    }
}
