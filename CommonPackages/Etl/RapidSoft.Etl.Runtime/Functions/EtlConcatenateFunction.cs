using System;
using System.Data;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Functions
{
    [Serializable]
    public sealed class EtlConcatenateFunction : EtlValueFunction
    {
        #region Constructors

        public EtlConcatenateFunction()
        {
        }

        public EtlConcatenateFunction(string sourceFieldName, EtlValueTranslation valueTranslation, string defaultValue)
        {
            this.SourceFieldName = sourceFieldName;
            this.SourceFieldTranslation = valueTranslation;
            this.DefaultValue = defaultValue;
        }

        #endregion

        #region Properties

        public bool IgnoreNulls
        {
            get;
            set;
        }

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

        public override object Evaluate(object value, IDataRecord sourceRecord)
        {
            if (sourceRecord == null)
            {
                throw new ArgumentNullException("sourceRecord");
            }

            var str0 = EtlValueConverter.ToString(value);
            var str1 = EtlValueTranslation.Evaluate(this.SourceFieldName, this.SourceFieldTranslation, sourceRecord, this.DefaultValue);

            if (str0 == null || str1 == null)
            {
                if (this.IgnoreNulls)
                {
                    if (str0 == null)
                    {
                        return str1;
                    }
                    else
                    {
                        return str0;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return string.Concat(str0, str1);
            }
        }

        #endregion
    }
}
