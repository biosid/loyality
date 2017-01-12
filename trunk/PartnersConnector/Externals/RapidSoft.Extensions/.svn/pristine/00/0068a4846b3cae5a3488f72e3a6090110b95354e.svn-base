using System.Collections.Generic;
using System.Linq;

namespace RapidSoft.Extensions
{
	using System;

	public static class LinqExtensions
    {
        public static bool IntersectsWith<T>(this IEnumerable<T> collection, IEnumerable<T> withWhat)
        {
            return collection.Any(item => withWhat.Contains(item));
        }

        public static bool IntersectsWith<T>(this IEnumerable<T> collection, params T[] withWhat)
        {
            return collection.Any(item => withWhat.Contains(item));
        }

        public static T[] MakeArray<T>(this T obj, params T[] others)
        {
            var asArray = new[] { obj };
            var retVal = asArray.Union(others).ToArray();
            return retVal;
        }

		/// <summary>
		/// Произведение коллекции <see cref="decimal"/>, аналог <see cref="Enumerable.Sum"/>.
		/// Если в коллекции нет ни одного элемента возвращает 1.
		/// </summary>
		/// <param name="source">
		/// Коллекция <see cref="decimal"/>
		/// </param>
		/// <returns>
		/// Произведение коллекции <see cref="decimal"/>
		/// </returns>
		public static decimal Product(this IEnumerable<decimal> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			return source.Aggregate<decimal, decimal>(1, (current, v) => current * v);
		}

        /// <summary>
        /// Null-безопастный аналог метода <see cref="Enumerable.Union{TSource}(System.Collections.Generic.IEnumerable{TSource},System.Collections.Generic.IEnumerable{TSource})"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> that contains the elements from both input sequences, excluding duplicates.
        /// </returns>
        /// <param name="first">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose distinct elements form the first set for the union.
        /// </param>
        /// <param name="second">
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose distinct elements form the second set for the union.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements of the input sequences.
        /// </typeparam>
        public static IEnumerable<TSource> SafeUnion<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            return (first ?? new TSource[0]).Union(second ?? new TSource[0]);
        }

        /// <summary>
        /// Метод форматирования элементов перечисления в строку, используется для логгирования перечислений.
        /// Если элементов в перечислении больше чем <paramref name="first"/>, то они заменяются строкой "...".
        /// </summary>
        /// <param name="source">
        /// Коллекция элементов.
        /// </param>
        /// <param name="first">
        /// Указывает какое кол-во первых элементов перечисления отобразить в выходной строке.
        /// </param>
        /// <typeparam name="T">
        /// Тип элементов перечисления.
        /// </typeparam>
        /// <returns>
        /// Сформированная строка.
        /// </returns>
        public static string ToFormattedString<T>(this IEnumerable<T> source, int first = 5)
        {
            if (source == null)
            {
                return "null";
            }

            var temp = source.Take(first + 1).Select(x => "{" + (x == null ? "null" : x.ToString()) + "}").ToArray();

            if (temp.Length == 6)
            {
                temp[6] = "...";
            }

            return "[" + string.Join(", ", temp) + "]";
        }

        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int size)
        {
            TSource[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                {
                    bucket = new TSource[size];
                }

                bucket[count++] = item;
                if (count != size)
                {
                    continue;
                }

                yield return bucket;

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
            {
                yield return bucket.Take(count);
            }
        }
    }
}