using System.Net;

namespace RapidSoft.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    public static class StringExtensions
    {
        public static string SafeTrim(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Trim();
        }

        public static string GetFirst(this string str, int lenght)
        {
            if (str == null)
            {
                return null;
            }
            return str.Length >= lenght ? str.Substring(0, lenght) : str;
        }

        /// <summary>
        /// Аналог string.IsNullOrWhiteSpace из .NET 4.0, но реализованный как метод-расширитель.
        /// </summary>
        /// <param name="value">Анализируемая строка</param>
        /// <returns><c>true</c> если <paramref name="value"/> равнен <c>null</c> или содержит только пробельные символы</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return value == null || value.All(char.IsWhiteSpace);
        }

        /// <summary>
        /// Удаляет теги из строки
        /// </summary>
        /// <param name="value">строка</param>
        /// <returns></returns>
        public static string StripTags(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(value);
            var noContent = doc.DocumentNode.SelectNodes("//style|//script|//noscript|//comment()");
            if (noContent != null)
            {
                foreach (var node in noContent.ToArray())
                {
                    node.Remove();
                }
            }
            return doc.DocumentNode.InnerText.Trim();
        }

        /// <summary>
        /// Выполняет десериализацию строки в объект типа <typeparamref name="T"/>.
        /// </summary>
        /// <param name="str">
        /// Исходная строка.
        /// </param>
        /// <param name="encoding">
        /// Кодировка. Не обязательный параметр.
        /// </param>
        /// <typeparam name="T">
        /// Тип получаемого объекта.
        /// </typeparam>
        /// <returns>
        /// Результирующий объект типа <typeparamref name="T"/>.
        /// </returns>
        public static T Deserialize<T>(this string str, Encoding encoding = null) where T : class
        {
            if (string.IsNullOrEmpty(str))
            {
                return default(T);
            }

            // return XmlSerializer.Deserialize<T>(str);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var memStream = new MemoryStream((encoding ?? Encoding.Unicode).GetBytes(str)))
            {
                return (T)serializer.Deserialize(memStream);
            }
        }
    }
}