using System;
using System.Data;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Functions
{
    [Serializable]
    public sealed class EtlTrimFunction : EtlValueFunction
    {
        #region Constructors

        public EtlTrimFunction()
        {
        }

        #endregion

        #region Properties

        public EtlTrimOptions Options
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

            if (value == null)
            {
                return null;
            }

            var result = Trim(EtlValueConverter.ToString(value));
            return result;
        }

        private string Trim(string str)
        {
            switch (this.Options)
            {
                case EtlTrimOptions.LeftAndRight:
                    return str.Trim();

                case EtlTrimOptions.Left:
                    return str.TrimStart();

                case EtlTrimOptions.Right:
                    return str.TrimEnd();

                default:
                    throw new InvalidOperationException(string.Format(Properties.Resources.UnknownTrimOption, this.Options));
            }
        }

        #endregion
    }
}
