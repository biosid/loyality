using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace RapidSoft.Etl.Runtime.DataSources.Xml
{
    internal static class SimpleXPathParser
    {
        public static string[] Parse(string simpleXPath)
        {
            if (string.IsNullOrEmpty(simpleXPath))
            {
                return new string[0];
            }

            var items = simpleXPath.Split('/');

            foreach (var item in items)
            {
                if (!IsElementName(item))
                {
                    throw new FormatException(string.Format("String \"{0}\" is not simple XPath expression", simpleXPath));
                }
            }

            return items;
        }

        private static bool IsElementName(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            if (!IsElementNameFirstChar(str[0]))
            {
                return false;
            }

            for (var i = 1; i < str.Length; i++)
            {
                if (!IsElementNameNonFirstChar(str[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsElementNameFirstChar(char ch)
        {
            return (char.IsLetter(ch) || (ch == '_'));
        }

        private static bool IsElementNameNonFirstChar(char ch)
        {
            return (char.IsLetterOrDigit(ch) || (ch == '_'));
        }
    }
}
