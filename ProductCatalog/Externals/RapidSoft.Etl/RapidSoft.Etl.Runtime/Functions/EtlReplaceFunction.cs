using System;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Functions
{
    [Serializable]
    public sealed class EtlReplaceFunction : EtlValueFunction
    {
        #region Constructors

        public EtlReplaceFunction()
        {
        }

        public EtlReplaceFunction(string pattern, string replacement)
        {
            this.Regex = pattern;
            this.Replacement = replacement;
        }

        #endregion

        #region Properties

        public string Regex
        {
            get;
            set;
        }

        public string Replacement
        {
            get;
            set;
        }

        public bool CaseSensitive
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

            if (string.IsNullOrEmpty(this.Regex) || string.IsNullOrEmpty(this.Replacement))
            {
                return null;
            }

            var str = EtlValueConverter.ToString(value);
            var options = GetRegexOptions();
            var regex = new Regex(this.Regex, options);
            var result = regex.Replace(str, this.Replacement ?? "");
            return result;
        }

        private RegexOptions GetRegexOptions()
        {
            if (this.CaseSensitive)
            {
                return RegexOptions.None;
            }
            else
            {
                return RegexOptions.IgnoreCase;                
            }
        }

        #endregion
    }
}
