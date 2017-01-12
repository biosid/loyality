namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values
{
    using System;
    using System.Globalization;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;

    /// <summary>
    /// Набор методов-расширителей для работы с <see cref="PromoAction.Api.FilterBuilder.value"/>.
    /// </summary>
    internal static class ValueExtensions
    {
        /// <summary>
        /// Разделитель значений массива
        /// </summary>
        private const char Splitter = ';';

        /// <summary>
        /// Формирует ключ для поиска значения в контексте или наборе алиасов.
        /// </summary>
        /// <param name="value">
        /// Переменная уравнения.
        /// </param>
        /// <returns>
        /// Сформированный ключ.
        /// </returns>
        public static string ContextKey(this value value)
        {
            var attr = value.attr.SingleOrDefault();

            if (attr == null)
            {
                throw new InvalidPredicateFormatException("Для узла \"value\" c типом \"attr\", не задан вложенный узел \"attr\"");
            }

            return string.Concat(attr.@object, ".", attr.name);
        }

        /// <summary>
        /// Возвращает значение <see cref="value.Text"/> как строку.
        /// </summary>
        /// <param name="value">
        /// Переменная уравнения.
        /// </param>
        /// <returns>
        /// Полученное значение.
        /// </returns>
        public static string ValueText(this value value)
        {
            if (value.Text.Length != 0 && value.Text[0] == null)
            {
                return null;
            }

            var textValue = string.Concat(value.Text);
            return textValue;
        }

        /// <summary>
        /// Возвращает тип переменной уравнения.
        /// </summary>
        /// <param name="value">
        /// Переменная уравнения.
        /// </param>
        /// <returns>
        /// Tип переменной уравнения.
        /// </returns>
        public static valueType ValueType(this value value)
        {
            var type = value.type;

            if (type == valueType.attr)
            {
                var attr = value.attr.SingleOrDefault();

                if (attr == null)
                {
                    throw new InvalidPredicateFormatException("Для узла \"value\" c типом \"attr\", не задан вложенный узел \"attr\"");
                }

                return attr.type;
            }

            return type;
        }

        public static object ConvertedValue(this string value, valueType type)
        {
            if (value == null || string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (value.IndexOf(Splitter) >= 0)
            {
                // NOTE: Имеем массив.
                var split = value.Split(new[] { Splitter });

                return split.Select(x => x.ConvertedValue(type)).ToArray();
            }

            switch (type)
            {
                case valueType.numeric:
                    {
                        return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                    }

                case valueType.@string:
                    {
                        return value;
                    }

                case valueType.boolean:
                    {
                        return Convert.ToBoolean(value);
                    }

                case valueType.datetime:
                    {
                        // NOTE: Считаем что дата отформатирована с помощью CultureInfo.InvariantCulture
                        return Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                    }

                default:
                    {
                        throw new NotSupportedException(string.Format("Тип данных {0} не поддерживается", type));
                    }
            }
        }
    }
}