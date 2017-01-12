using System;
using System.Data;
using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Functions
{
    [Serializable]
    public sealed class EtlSubstringFunction : EtlValueFunction
    {
        #region Constructors

        public EtlSubstringFunction()
        {
        }

        public EtlSubstringFunction(int skip, int length)
        {
            this.Skip = skip;
            this.Length = length;
        }

        #endregion

        #region Properties

        public int Skip
        {
            get;
            set;
        }

        public int Length
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

            if (this.Length <= 0)
            {
                return null;
            }

            if (value == null)
            {
                return null;
            }

            var str = EtlValueConverter.ToString(value);
            var skip = Math.Max(this.Skip, 0);
            var length = this.Length;

            if (skip >= str.Length)
            {
                return null;
            }

            if (skip + length > str.Length)
            {
                length = str.Length - skip;
            }

            var result = str.Substring(skip, length);
            return result;
        }

        #endregion
    }
}
