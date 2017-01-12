using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Functions
{
    [Serializable]
    public sealed class EtlMatchFunction : EtlValueFunction
    {
        #region Constructors

        public EtlMatchFunction()
        {
        }

        public EtlMatchFunction(EtlMatchRule rule)
            : this(rule, null, null)
        {
        }

        public EtlMatchFunction(EtlMatchRule rule0, EtlMatchRule rule1)
            : this(rule0, rule1, null)
        {
        }

        public EtlMatchFunction(EtlMatchRule rule0, EtlMatchRule rule1, EtlMatchRule rule2)
        {
            if (rule0 != null)
            {
                _rules.Add(rule0);
            }

            if (rule1 != null)
            {
                _rules.Add(rule1);
            }

            if (rule2 != null)
            {
                _rules.Add(rule2);
            }
        }

        public EtlMatchFunction(IEnumerable<EtlMatchRule> rules)
        {
            if (rules != null)
            {
                _rules.AddRange(rules);
            }
        }

        #endregion

        #region Properties

        [Editor(EtlComponentModelInfo.EtlCollectionModelEditorType, EtlComponentModelInfo.EtlCollectionModelEditorBase)]
        [XmlArrayItem("Rule")]
        public List<EtlMatchRule> Rules
        {
            [DebuggerStepThrough]
            get
            {
                return _rules;
            }
        }
        private List<EtlMatchRule> _rules = new List<EtlMatchRule>();

        #endregion

        #region Methods

        public override object Evaluate(object value, IDataRecord sourceRecord)
        {
            if (sourceRecord == null)
            {
                throw new ArgumentNullException("sourceRecord");
            }

            if (_rules.Count == 0)
            {
                return null;
            }

            var str = EtlValueConverter.ToString(value);

            foreach (var rule in _rules)
            {
                if (!string.IsNullOrEmpty(rule.Regex))
                {
                    var options = GetRegexOptions(rule);
                    var regex = new Regex(rule.Regex, options);
                    if (regex.IsMatch(str))
                    {
                        if (rule.Result == null)
                        {
                            return null;
                        }
                        else
                        {
                            var result = EtlValueTranslation.Evaluate(rule.Result.SourceFieldName, rule.Result.SourceFieldTranslation, sourceRecord, rule.Result.DefaultValue);
                            return result;
                        }
                    }
                }
            }

            return null;
        }

        private RegexOptions GetRegexOptions(EtlMatchRule rule)
        {
            if (rule.CaseSensitive)
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
