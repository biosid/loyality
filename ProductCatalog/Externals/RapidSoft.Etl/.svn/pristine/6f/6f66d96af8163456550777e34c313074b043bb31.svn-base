using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

using RapidSoft.Etl.Logging;
using System.Globalization;

namespace RapidSoft.Etl.Runtime.Functions
{
    [Serializable]
    public sealed class EtlDecodeFunction : EtlValueFunction
    {
        #region Constructors

        public EtlDecodeFunction()
        {
        }

        public EtlDecodeFunction(EtlDecodeRule rule)
            : this(rule, null, null)
        {
        }

        public EtlDecodeFunction(EtlDecodeRule rule0, EtlDecodeRule rule1)
            : this(rule0, rule1, null)
        {
        }

        public EtlDecodeFunction(EtlDecodeRule rule0, EtlDecodeRule rule1, EtlDecodeRule rule2)
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

        public EtlDecodeFunction(IEnumerable<EtlDecodeRule> rules)
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
        public List<EtlDecodeRule> Rules
        {
            [DebuggerStepThrough]
            get
            {
                return _rules;
            }
        }
        private List<EtlDecodeRule> _rules = new List<EtlDecodeRule>();

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
                if (string.Equals(rule.Value, str, GetStringComparison(rule)))
                {
                    return rule.Result;
                }
            }

            return null;
        }

        private StringComparison GetStringComparison(EtlDecodeRule rule)
        {
            if (rule.CaseSensitive)
            {
                return StringComparison.InvariantCulture;
            }
            else
            {
                return StringComparison.InvariantCultureIgnoreCase;
            }
        }

        #endregion
    }
}
