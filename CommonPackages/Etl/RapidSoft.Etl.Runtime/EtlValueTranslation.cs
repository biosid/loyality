using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace RapidSoft.Etl.Runtime
{
    using RapidSoft.Etl.Runtime.DataSources.Csv;

    [Serializable]
    [TypeConverter(typeof(EtlValueTranslationObjectConverter))]
    public class EtlValueTranslation
    {
        #region Constructors

        public EtlValueTranslation()
        {
        }

        public EtlValueTranslation(EtlValueFunction func)
            : this(func, null, null)
        {
        }

        public EtlValueTranslation(EtlValueFunction func0, EtlValueFunction func1)
            : this(func0, func1, null)
        {
        }

        public EtlValueTranslation(EtlValueFunction func0, EtlValueFunction func1, EtlValueFunction func2)
        {
            if (func0 != null)
            {
                this.Functions.Add(func0);
            }

            if (func1 != null)
            {
                this.Functions.Add(func1);
            }

            if (func2 != null)
            {
                this.Functions.Add(func2);
            }
        }

        public EtlValueTranslation(IEnumerable<EtlValueFunction> functions)
        {
            if (functions != null)
            {
                _functions.AddRange(functions);
            }
        }

        #endregion

        #region Properties

        [Editor(EtlComponentModelInfo.EtlCollectionModelEditorType, EtlComponentModelInfo.EtlCollectionModelEditorBase)]
        public List<EtlValueFunction> Functions
        {
            [DebuggerStepThrough]
            get
            {
                return _functions;
            }
        }
        private List<EtlValueFunction> _functions = new List<EtlValueFunction>();

        #endregion

        #region Methods

        public static object Evaluate(string sourceFieldName, EtlValueTranslation sourceFieldTranslation, IDataRecord sourceRecord)
        {
            return Evaluate(sourceFieldName, sourceFieldTranslation, sourceRecord, null);
        }

        public static object Evaluate(string sourceFieldName, EtlValueTranslation sourceFieldTranslation, IDataRecord sourceRecord, string defaultValue)
        {
            if (sourceRecord == null)
            {
                throw new ArgumentNullException("sourceRecord");
            }

            var sourceFieldValue = GetTranslatedFieldValue(sourceFieldName, sourceFieldTranslation, sourceRecord);
            if (sourceFieldValue == null || sourceFieldValue == DBNull.Value)
            {
                return defaultValue;
            }
            else
            {
                return sourceFieldValue;
            }
        }

        private static object GetTranslatedFieldValue(string sourceFieldName, EtlValueTranslation sourceFieldTranslation, IDataRecord sourceRecord)
        {
            if (string.IsNullOrEmpty(sourceFieldName))
            {
                return null;
            }

            if (sourceFieldName.StartsWith("@"))
            {
                // NOTE: Получение спец значения из ридера
                var retVal = GetSpecValue(sourceFieldName, sourceRecord);
                return retVal;
            }

            var sourceFieldValue = sourceRecord[sourceFieldName];
            var translatedValue = sourceFieldValue;
            if (sourceFieldTranslation != null)
            {
                try
                {
                    foreach (var func in sourceFieldTranslation.Functions)
                    {
                        translatedValue = func.Evaluate(translatedValue, sourceRecord);
                    }
                    return translatedValue;
                }
                catch (Exception exc)
                {
                    throw new InvalidOperationException
                    (
                        string.Format
                        (
                            Properties.Resources.ValueTranslationError,
                            sourceFieldName,
                            sourceFieldValue
                        ),
                        exc
                    );
                }
            }
            return translatedValue;
        }

        private static object GetSpecValue(string sourceFieldName, IDataRecord sourceRecord)
        {
            switch (sourceFieldName)
            {
                case "@RowIndex":
                {
                    var csvReader = sourceRecord as CsvReader;

                    if (csvReader == null)
                    {
                        return null;
                    }

                    var currentRecordIndex = csvReader.CurrentRecordIndex;
                    var skip = csvReader.CommentAndEmptyLineSkipCount;

                    // NOTE: так как заголовки занимают одну строчку если он есть, то +1
                    currentRecordIndex = csvReader.HasHeaders ? currentRecordIndex + 1 : currentRecordIndex;

                    // NOTE: Так как индекс начинается с 0, то +1 и + кол-во пропущенных пустых строк
                    var line = currentRecordIndex + 1 + skip;

                    return line;
                }
                
                default:
                {
                    var mess = string.Format(Properties.Resources.InvalidSpecSourceField, sourceFieldName);
                    throw new InvalidOperationException(mess);
                }
            }
        }

        private object GetSourceFieldValue(string sourceFieldName, IDataRecord sourceRecord)
        {
            try
            {
                return sourceRecord[sourceFieldName];
            }
            catch (IndexOutOfRangeException exc)
            {
                throw new InvalidOperationException
                (
                    string.Format
                    (
                        Properties.Resources.ValueTranslationSourceFieldNotFound,
                        sourceFieldName
                    ),
                    exc
                );
            }
        }

        #endregion

        #region Nested classes

        public sealed class EtlValueTranslationObjectConverter : ExpandableObjectConverter
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
                    var obj = (EtlValueTranslation)value;

                    if (obj != null)
                    {
                        return obj.GetType().Name;
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

                return new EtlValueTranslation();
            }
        }

        #endregion
    }
}
