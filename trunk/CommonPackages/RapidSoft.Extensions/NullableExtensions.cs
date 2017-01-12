namespace RapidSoft.Extensions
{
    using System;

    public static class NullableExtensions
    {
        public static bool IsNullOr(this bool? val, bool orWhat)
        {
            return val == null || val == orWhat;
        }

        public static bool IsNullOr(this string val, string orWhat)
        {
            return val == null || val == orWhat;
        }

        public static bool IsNullOr(this int? val, int orWhat)
        {
            return val == null || val == orWhat;
        }

        public static bool IsNullOr(this float? val, float orWhat)
        {
            return val == null || val == orWhat;
        }

        public static bool IsNullOr(this double? val, double orWhat)
        {
            return val == null || val == orWhat;
        }

        public static bool IsNullOr(this decimal? val, decimal orWhat)
        {
            return val == null || val == orWhat;
        }

        /// <summary>
        /// Метод генерирует <see cref="ArgumentNullException"/>, если применяется на объекте равном <c>null</c>.
        /// Метод используется для записи в одну строку проверки входных параметров.
        /// Внимание: метод слегка искажает stack trace.
        /// </summary>
        /// <param name="obj">
        /// Проверяемый объект.
        /// </param>
        /// <param name="paramName">
        /// Название параметра.
        /// </param>
        public static void ThrowIfNull(this object obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Метод генерирует <see cref="ArgumentException"/>, если применяется на строке равном <c>null</c> или строка содержит только пробельные символы.
        /// </summary>
        /// <param name="str">
        /// Анализируемая строка.
        /// </param>
        /// <param name="paramName">
        /// Название параметра.
        /// </param>
        public static void ThrowIfNullOrWhiteSpace(this string str, string paramName)
        {
            if (str.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(string.Format("Параметр \"{0}\" не может быть null или пустой строкой"));
            }
        }
    }
}