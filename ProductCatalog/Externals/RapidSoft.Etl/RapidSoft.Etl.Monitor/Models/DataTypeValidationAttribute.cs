using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace RapidSoft.Etl.Monitor.Models
{
    public class DataTypeValidationAttribute : ValidationAttribute
    {
        public DataType DataType
        {
            get;
            set;
        }

        public DataTypeValidationAttribute(DataType dataType)
        {
            DataType = dataType;
        }

        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            switch (DataType)
            {
                case DataType.Int:
                    int intRes;
                    return int.TryParse(value.ToString(), out intRes);
                case DataType.Decimal:
                    decimal decimalRes;
                    return Decimal.TryParse(value.ToString(), out decimalRes);
                case DataType.DateTime:
                    DateTime datetimeRes;
                    return DateTime.TryParse(value.ToString(), out datetimeRes);
            }
            throw new NotSupportedException(DataType + " is not supported");
        }
    }
}