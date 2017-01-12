namespace RapidSoft.VTB24.BankConnector.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class EnumerableExtension
    {
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        public static Tuple<IList<TSource>, IList<TSource>> Split<TSource>(
            this TSource[] enumerable, Func<TSource, bool> predic)
        {
            return
                enumerable.Aggregate(
                    new Tuple<IList<TSource>, IList<TSource>>(new List<TSource>(), new List<TSource>()), 
                    (res, source) =>
                    {
                        if (predic(source))
                        {
                            res.Item1.Add(source);
                        }
                        else
                        {
                            res.Item2.Add(source);
                        }

                        return res;
                    }, 
                    tuple => tuple);
        }
    }
}