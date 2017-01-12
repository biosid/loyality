using System;
using System.Collections.Generic;
using System.Linq;

namespace Vtb24.Site.Services.Infrastructure
{
    /// <summary>
    /// Склонение по числам
    /// </summary>
    public static class Pluralizer
    {

        #region Склонение

        public static T Decline<T>(this long num, T singular, T plural, T plural2 = null) where T : class
        {
            if (plural2 == null)
            {
                return num % 10 == 1 && num % 100 != 11 ? singular : plural;
            }

            var x = num % 100;
            if (x >= 5 && x <= 20)
            {
                return plural2;
            }

            switch (num % 10)
            {
                case 1:
                    return singular;
                case 2:
                case 3:
                case 4:
                    return plural;
                default:
                    return plural2;
            }
        }

        public static T Decline<T>(this int num, T singular, T plural, T plural2 = null) where T : class
        {
            return Decline((long)num, singular, plural, plural2);
        }

        public static T Decline<T>(this short num, T singular, T plural, T plural2 = null) where T : class
        {
            return Decline((long)num, singular, plural, plural2);
        }

        public static T Decline<T>(this byte num, T singular, T plural, T plural2 = null) where T : class
        {
            return Decline((long)num, singular, plural, plural2);
        }

        public static T Decline<T>(this decimal num, T singular, T plural, T plural2 = null) where T : class
        {
            return num.Equals((long)num) ? Decline((long)num, singular, plural, plural2) : plural;
        }

        public static T Decline<T>(this double num, T singular, T plural, T plural2 = null) where T : class
        {
            // Код скопирован из метода для Decimal, т.к. приводить double к decimal небезопасно
            return num.Equals((long)num) ? Decline((long)num, singular, plural, plural2) : plural;
        }

        public static T Decline<T>(this float num, T singular, T plural, T plural2 = null) where T : class
        {
            return Decline((double)num, singular, plural, plural2);
        }

        #endregion


        #region Шаблонизация

        public static string Pluralize(this long num, string singular, string plural, string plural2 = null, params ISpecialCase<long>[] special)
        {
            return Pluralize(num, () => Decline(num, singular, plural, plural2), special);
        }

        public static string Pluralize(this int num, string singular, string plural, string plural2 = null, params ISpecialCase<int>[] special)
        {
            return Pluralize(num, () => Decline(num, singular, plural, plural2), special);
        }

        public static string Pluralize(this short num, string singular, string plural, string plural2 = null, params ISpecialCase<short>[] special)
        {
            return Pluralize(num, () => Decline(num, singular, plural, plural2), special);
        }

        public static string Pluralize(this byte num, string singular, string plural, string plural2 = null, params ISpecialCase<byte>[] special)
        {
            return Pluralize(num, () => Decline(num, singular, plural, plural2), special);
        }

        public static string Pluralize(this decimal num, string singular, string plural, string plural2 = null, params ISpecialCase<decimal>[] special)
        {
            return Pluralize(num, () => Decline(num, singular, plural, plural2), special);
        }

        public static string Pluralize(this double num, string singular, string plural, string plural2 = null, params ISpecialCase<double>[] special)
        {
            return Pluralize(num, () => Decline(num, singular, plural, plural2), special);
        }

        public static string Pluralize(this float num, string singular, string plural, string plural2 = null, params ISpecialCase<float>[] special)
        {
            return Pluralize(num, () => Decline(num, singular, plural, plural2), special);
        }

        #endregion


        #region Приватные методы

        private static string Pluralize<T>(T num, Func<string> plur, IEnumerable<ISpecialCase<T>> specialCases)
            where T : struct
        {
            var specialCase = GetSpecialCase(num, specialCases);
            if (specialCase != null)
            {
                return ParseTemplate(num, specialCase.Value);
            }
            var declension = plur();
            return ParseTemplate(num, declension);
        }

        private static ISpecialCase<T> GetSpecialCase<T>(T num, IEnumerable<ISpecialCase<T>> cases)
            where T : struct
        {
            return cases == null ? null : cases.FirstOrDefault(specialCase => specialCase.IsCase(num));
        }

        private static string ParseTemplate<T>(T num, string template) where T : struct
        {
            var t = template.Replace("{1", "{0").Replace("{2", "{0").Replace("{5", "{0");
            return t.Contains("{0") ? string.Format(t, num) : template;
        }

        #endregion


        #region Специальные случаи

        public static ISpecialCase<T> If<T>(T num, string str) where T : struct
        {
            return new ConstantCase<T>(num, str);
        }

        public static ISpecialCase<T> If<T>(bool condition, string str) where T : struct
        {
            return new ConditionCase<T>(condition, str);
        }

        public interface ISpecialCase<in T>
        {
            string Value { get; }
            bool IsCase(T num);
        }

        private class ConstantCase<T> : ISpecialCase<T> where T : struct
        {
            private readonly T _const;
            private readonly string _str;

            public ConstantCase(T refference, string str)
            {
                _const = refference;
                _str = str;
            }

            public string Value
            {
                get { return _str; }
            }

            public bool IsCase(T num)
            {
                return _const.Equals(num);
            }
        }

        private class ConditionCase<T> : ISpecialCase<T> where T : struct
        {
            private readonly bool _condition;
            private readonly string _str;

            public ConditionCase(bool condition, string str)
            {
                _condition = condition;
                _str = str;
            }

            public string Value
            {
                get { return _str; }
            }

            public bool IsCase(T num)
            {
                return _condition;
            }
        }

        #endregion
    }
}