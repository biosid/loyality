using System;
using System.Collections.Generic;
using System.Linq;

namespace Vtb24.Site.Infrastructure
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            return list.Select((item, index) => new { index, item })
                       .GroupBy(x => x.index % parts)
                       .Select(x => x.Select(y => y.item));
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Select(x => Tuple.Create(x, NextRandom()))
                         .OrderBy(t => t.Item2)
                         .Select(t => t.Item1);
        }

        public static void MixToList<T>(this IEnumerable<T> source, List<object> list)
        {
            var items = source.Take(list.Count).ToArray();

            list.RemoveRange(list.Count - items.Length, items.Length);

            foreach (var item in items)
            {
                list.Insert(NextRandom(0, list.Count + 1), item);
            }
        }

        private static readonly Random Random = new Random();
        private static readonly object RandomLock = new object();

        private static int NextRandom()
        {
            lock (RandomLock)
            {
                return Random.Next();
            }
        }

        private static int NextRandom(int minValue, int maxValue)
        {
            lock (RandomLock)
            {
                return Random.Next(minValue, maxValue);
            }
        }
    }
}
