using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidSoft.Extensions
{
    public static class RandomElementExtensions
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        public static T GetRandomElement<T>(this IEnumerable<T> collection)
        {
            var limit = collection.Count();
            if (limit == 0)
            {
                throw new Exception("Can not get random element: collection is empty!");
            }

            return collection.ElementAt(limit);
        }

        public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> collection, int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("Elements count must be greater than zero", "count");
            }

            var limit = collection.Count();
            IEnumerable<int> indexes;

            if (count >= limit)
            {
                indexes = GetRange(limit);
            }
            else if (count * 2 > limit)
            {
                var excludeCount = limit - count;
                var excludedIndexes = GenerateUniqueIndexes(excludeCount, limit);
                indexes = GetRange(limit).Where(i => !excludedIndexes.Contains(i));
            }
            else
            {
                indexes = GenerateUniqueIndexes(count, limit);
            }

            return indexes.OrderBy(i => i).Select(i => collection.ElementAt(i));
        }

        private static IEnumerable<int> GenerateUniqueIndexes(int count, int maxValue)
        {
            
            var indexes = new Dictionary<int,string>();

            while (indexes.Count < count)
            {
                indexes[Random.Next(maxValue)] = String.Empty;
            }

            return indexes.Keys;
        }

        private static IEnumerable<int> GetRange(int to)
        {
            for (var i = 0; i < to; i++)
            {
                yield return i;
            }
        }
    }
}