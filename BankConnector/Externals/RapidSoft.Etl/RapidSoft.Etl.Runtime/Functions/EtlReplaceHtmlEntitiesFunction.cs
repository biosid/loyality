using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using RapidSoft.Etl.Logging;

namespace RapidSoft.Etl.Runtime.Functions
{
    [Serializable]
    public sealed class EtlReplaceHtmlEntitiesFunction : EtlValueFunction
    {
        #region Fields

        private const string UNKNOWN_ENTITY_REPLACEMENT = "?";

        private static readonly Dictionary<string, string> KnownCharEntities = new Dictionary<string, string>
        {
            { "nbsp", " " },
            { "copy", "(C)" },
            { "reg", "(R)" },
            { "trade", "(TM)" },
            { "quot", "\"" },
            { "ndash", "-" },
            { "mdash", "-" },
            { "lsquo", "'" },
            { "rsquo", "'" },
            { "ldquo", "\"" },
            { "rdquo", "\"" },
            { "laquo", "\"" },
            { "raquo", "\"" }
        };

        private readonly Regex _regex = new Regex("&(#[0-9]+|#[xX][0-9a-fA-F]+|[:A-Z_a-z][:A-Z_a-z.0-9-]*);");

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

            var str = EtlValueConverter.ToString(value);

            if (str == null)
            {
                return null;
            }

            var result = _regex.Replace(str, ReplaceEntity);

            return result;
        }

        private static string ReplaceEntity(Match match)
        {
            try
            {
                if (match.Value[1] == '#')
                {
                    int code;
                    if (match.Value[2] == 'x' || match.Value[2] == 'X')
                    {
                        var hexStr = match.Value.Substring(3, match.Value.Length - 4);
                        if (!int.TryParse(hexStr, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out code))
                        {
                            return UNKNOWN_ENTITY_REPLACEMENT;
                        }
                    }
                    else
                    {
                        var decStr = match.Value.Substring(2, match.Value.Length - 3);
                        if (!int.TryParse(decStr, NumberStyles.None, CultureInfo.InvariantCulture, out code))
                        {
                            return UNKNOWN_ENTITY_REPLACEMENT;
                        }
                    }

                    if (code == 38)
                    {
                        return " & ";
                    }

                    if (code >= 32 && code < 127)
                    {
                        return new string((char)code, 1);
                    }
                }
                else
                {
                    var entityName = match.Value.Substring(1, match.Value.Length - 2);
                    if (entityName == "amp")
                    {
                        return " & ";
                    }

                    string result;
                    return KnownCharEntities.TryGetValue(entityName, out result) ? result : UNKNOWN_ENTITY_REPLACEMENT;
                }

                return UNKNOWN_ENTITY_REPLACEMENT;
            }
            catch (Exception)
            {
                return UNKNOWN_ENTITY_REPLACEMENT;
            }
        }

        #endregion
    }
}
