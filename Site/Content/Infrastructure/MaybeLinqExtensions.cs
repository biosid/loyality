using System;
using System.Collections.Generic;
using System.Linq;

namespace Vtb24.Site.Content.Infrastructure
{
    internal static class MaybeLinqExtensions
    {
        public static IEnumerable<T> MaybeTake<T>(this IEnumerable<T> en, int? top)
        {
            if (en == null)
            {
                return null;
            }

            return top.HasValue ? en.Take(top.Value) : en;
        }

        public static IEnumerable<T> MaybeSkip<T>(this IEnumerable<T> en, int? skip)
        {
            if (en == null)
            {
                return null;
            }

            return skip.HasValue ? en.Skip(skip.Value) : en;
        }

        public static T[] MaybeToArray<T>(this IEnumerable<T> en)
        {
            return en == null ? null : en.ToArray();
        }

        public static IEnumerable<TResult> MaybeSelect<TSource, TResult>(this IEnumerable<TSource> en, Func<TSource, TResult> selector)
        {
            return en == null ? null : en.Select(selector);
        }

        public static IEnumerable<TSource> MaybeWhere<TSource>(this IEnumerable<TSource> en, Func<TSource, bool> selector)
        {
            return en == null ? null : en.Where(selector);
        }

        public static TResult? MaybeInvoke<TInput, TResult>(this TInput? input, Func<TInput, TResult> func)
            where TResult : struct
            where TInput : struct
        {
            return input.HasValue ? func(input.Value) : (TResult?)null;
        }

        public static T MaybeFirstOrDefault<T>(this IEnumerable<T> en) where T : class
        {
            return en == null ? null : en.FirstOrDefault();
        }
    }
}