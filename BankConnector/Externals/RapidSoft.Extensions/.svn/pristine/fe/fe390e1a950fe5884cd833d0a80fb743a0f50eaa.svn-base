namespace RapidSoft.Extensions
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web.Script.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Класс с методами-расширителями применимых на <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Выполняет сериализацию объекта в строку типа <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Тип сериализуемого объекта.
        /// </typeparam>
        /// <param name="obj">
        /// Сериализуемый объект
        /// </param>
        /// <param name="encoding">
        /// Кодировка. Не обязательный параметр.
        /// </param>
        /// <param name="omitNamespace">
        /// Пропускать ли пространство имен.
        /// </param>
        /// <returns>
        /// XML представление объекта
        /// </returns>
        public static string Serialize<T>(this T obj, Encoding encoding = null, bool omitNamespace = false)
        {
            var ns = new XmlSerializerNamespaces();
            if (omitNamespace)
            {
                ns.Add(string.Empty, string.Empty);
            }

            return Serialize(obj, encoding, ns);
        }

        private static XmlSerializerNamespaces GetDefaultNamespaces()
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, @"xmlns=""http://tempuri.org/XMLSchema.xsd""");
            return ns;
        }

        private static string Serialize<T>(T obj, Encoding encoding, XmlSerializerNamespaces ns = null)
        {
            if (obj == null)
            {
                return null;
            }

            ns = ns ?? GetDefaultNamespaces();

            var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());

            using (var sw = new InternalStringWriter(encoding ?? Encoding.Unicode))
            {
                serializer.Serialize(sw, obj, ns);

                return sw.ToString();
            }
        }

        public static string JsonSerialize<T>(this T obj)
        {
            if (obj == null)
            {
                return null;
            }

            var serializer = new JavaScriptSerializer();

            return serializer.Serialize(obj);
        }

        /// <summary>
        /// Внутренная реализация <see cref="StringWriter"/>, позволяющая управлять кодировкой
        /// </summary>
        public sealed class InternalStringWriter : StringWriter
        {
            /// <summary>
            /// The Encoding in which the output is written.
            /// </summary>
            private readonly Encoding encoding;

            /// <summary>
            /// Initializes a new instance of the <see cref="InternalStringWriter"/> class.
            /// </summary>
            /// <param name="encoding">
            /// The Encoding in which the output is written.
            /// </param>
            public InternalStringWriter(Encoding encoding)
            {
                this.encoding = encoding;
            }

            /// <summary>
            /// Gets the <see cref="T:System.Text.Encoding"/> in which the output is written.
            /// </summary>
            /// <returns>
            /// The Encoding in which the output is written.
            /// </returns>
            public override Encoding Encoding
            {
                get
                {
                    return this.encoding;
                }
            }
        }

        public static string ToStringOrNull(this object obj)
        {
            return obj == null ? null : obj.ToString();
        }

        public static string ToStringOrEmpty(this object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        public static T1 Maybe<T0, T1>(this T0 value, Func<T0, T1> f) where T0 : class
        {
            if (value == null)
            {
                return default(T1);
            }

            return f(value);
        }

        public static T1 Maybe<T0, T1>(this T0 value, Func<T0, T1> f, T1 defaultValue) where T0 : class
        {
            if (value == null)
            {
                return defaultValue;
            }

            var result = f(value);

            if (result == null)
            {
                return defaultValue;
            }
            else
            {
                return result;
            }
        }
    }
}
