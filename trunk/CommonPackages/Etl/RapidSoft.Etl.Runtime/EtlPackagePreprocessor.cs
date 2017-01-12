using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Security;
using System.Xml;
using System.Xml.Serialization;

using RapidSoft.Etl.Logging;


namespace RapidSoft.Etl.Runtime
{
    internal sealed class EtlPackagePreprocessor
    {
        #region Constructors

        public EtlPackagePreprocessor()
        {
        }

        #endregion

        #region Constants

        private const int PROCESSED_STRING_LENGTH_FACTOR = 2;
        private const int VARIABLE_NAME_AVG_LENGTH = 10;

        private const char VARIABLE_PREFIX = '$';
        private const char VARIABLE_LP = '(';
        private const char VARIABLE_RP = ')';

        #endregion

        #region Fields

        public static readonly string VariableTokenFormat = "$({0})";

        #endregion

        #region Methods

        public EtlPackage PreprocessPackage(EtlPackage package, EtlVariable[] variables)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            var ser = new EtlPackageXmlSerializer();
            var xml = ser.Serialize(package);
            xml = PreprocessPackageXml(xml, variables);
            
            var result = ser.Deserialize(xml);
            return result;
        }

        private string PreprocessPackageXml(string packageXml, EtlVariable[] variables)
        {
            if (packageXml == null)
            {
                throw new ArgumentNullException("packageXml");
            }

            var result = new StringBuilder(packageXml.Length * PROCESSED_STRING_LENGTH_FACTOR);
            var variableNameBuf = new StringBuilder(VARIABLE_NAME_AVG_LENGTH);
            var i = 0;

            while (i < packageXml.Length)
            {
                var ch = packageXml[i];
                if (ch == VARIABLE_PREFIX)
                {
                    if (TryReadVariableName(packageXml, ref i, variableNameBuf))
                    {
                        var variableName = variableNameBuf.ToString();
                        var variable = FindVariable(variables, variableName);
                        if (variable == null)
                        {
                            throw new EtlPackageException(string.Format(Properties.Resources.VariableNotFound, variableName));
                        }

                        result.Append(EncodeXmlString(variable.Value));
                    }
                    else
                    {
                        result.Append(variableNameBuf);
                    }

                    variableNameBuf.Length = 0;
                }
                else
                {
                    result.Append(ch);
                    i++;
                }
            }

            return result.ToString();
        }

        private static bool TryReadVariableName(string packageXml, ref int position, StringBuilder output)
        {
            var startPosition = position;

            var ch = packageXml[position];
            position++;
            if (ch != VARIABLE_PREFIX)
            {
                output.Append(ch);
                return false;
            }

            if (position >= packageXml.Length)
            {
                return false;
            }

            ch = packageXml[position];
            position++;
            if (ch != VARIABLE_LP)
            {
                output.Append(VARIABLE_PREFIX);
                output.Append(ch);
                return false;
            }

            for (var i = position; i < packageXml.Length; i++)
            {
                ch = packageXml[i];
                if (ch == VARIABLE_RP)
                {
                    position = i + 1;
                    return true;
                }
                else
                {
                    output.Append(ch);
                }
            }

            throw new EtlPackageException(string.Format(Properties.Resources.VariableTokenHasNoClosingParenthese, startPosition));
        }

        private static EtlVariable FindVariable(EtlVariable[] variables, string variableName)
        {
            foreach (var variable in variables)
            {
                if (string.Equals(variable.Name, variableName, StringComparison.InvariantCulture))
                {
                    return variable;
                }
            }

            return null;
        }

        private static string EncodeXmlString(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return xml;
            }

            //replace "&" with "&amp;"
            //replace "<" with "&lt;"
            //replace ">" with "&gt;"
            //replace "\"" with "&quot;"
            //replace "'" with "&apos;"
            return SecurityElement.Escape(xml);
        }

        #endregion
    }
}
