using System.Collections.Generic;

namespace Rapidsoft.VTB24.Reports.Statistics.Helpers
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> SplitBatch<T>(this IEnumerable<T> source, int batchSize)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return YieldBatchElements(enumerator, batchSize - 1);
                }
            }
        }

        private static IEnumerable<T> YieldBatchElements<T>(IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;
            var i = 0;
            while (i < batchSize && source.MoveNext())
            {
                ++i;
                yield return source.Current;
            }
        } 
    }
}
