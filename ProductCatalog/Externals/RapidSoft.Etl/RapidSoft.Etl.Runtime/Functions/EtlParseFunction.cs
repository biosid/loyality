using System;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using RapidSoft.Etl.Logging;
using System.Globalization;

namespace RapidSoft.Etl.Runtime.Functions
{
    [Serializable]
    public sealed class EtlParseFunction : EtlValueFunction
    {
        #region Constructors

        public EtlParseFunction()
        {
        }

        public EtlParseFunction(string pattern, string groupName)
        {
            this.Regex = pattern;
            this.GroupName = groupName;
        }

        #endregion

        #region Properties

        public string Regex
        {
            get;
            set;
        }

        public string GroupName
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

            if (string.IsNullOrEmpty(this.Regex) || string.IsNullOrEmpty(this.GroupName))
            {
                return null;
            }

            var str = EtlValueConverter.ToString(value);
            var options = GetRegexOptions();
            var regex = new Regex(this.Regex, options);
            var matches = regex.Match(str);
            if (matches == null)
            {
                return null;
            }

            var group = matches.Groups[this.GroupName];
            if (group == null)
            {
                return null;
            }
            else
            {
                if (string.IsNullOrEmpty(group.Value))
                {
                    return null;
                }
                else
                {
                    return group.Value;
                }
            }
        }

        private RegexOptions GetRegexOptions()
        {
            if (!this.CaseSensitive)
            {
                return RegexOptions.IgnoreCase;
            }

            return RegexOptions.None;
        }

        #endregion
    }
}
