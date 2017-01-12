using System;
using System.Collections.Generic;
using System.Linq;

namespace Vtb24.Site.Models.Catalog.Helpers
{
    public static class CatalogRubricsSplitter
    {
        public static CatalogRubricModel[][] SplitToColumns(this CatalogRubricModel[] rubrics, int minColumnSize)
        {
            var rubricsSizes = rubrics.Select(GetSize).ToArray();

            var split = FindBestSplit(rubricsSizes, minColumnSize);

            return rubrics.Slices(split).ToArray();
        }

        private const int MAX_COLUMNS_COUNT = 3;

        private static IEnumerable<int> FindBestSplit(int[] sizes, int minColumnSize)
        {
            var bestSize = sizes.Sum();
            var bestSplit = new[] { sizes.Length };

            if (bestSize <= MAX_COLUMNS_COUNT)
            {
                return bestSplit;
            }

            foreach (var split in GetSplits(sizes.Length, MAX_COLUMNS_COUNT).Select(s => s.ToArray()))
            {
                var splitSize = TrySplit(sizes, split, bestSize);

                if (!splitSize.HasValue)
                {
                    continue;
                }

                if (splitSize.Value <= minColumnSize)
                {
                    return split;
                }

                bestSize = splitSize.Value;
                bestSplit = split;
            }

            return bestSplit;
        }

        private static int? TrySplit(int[] sizes, IEnumerable<int> split, int bestSplitSize)
        {
            var splitSize = 0;

            var skip = 0;

            foreach (var slice in split)
            {
                var sliceSize = 0;

                foreach (var size in Enumerable.Range(skip, slice).Select(i => sizes[i]))
                {
                    sliceSize += size;

                    if (sliceSize >= bestSplitSize)
                    {
                        return null;
                    }
                }

                if (sliceSize > splitSize)
                {
                    splitSize = sliceSize;
                }

                skip += slice;
            }

            return splitSize;
        }

        private static IEnumerable<IEnumerable<int>> GetSplits(int count, int maxColumnsCount)
        {
            if (count < 1)
            {
                return Enumerable.Empty<IEnumerable<int>>();
            }

            if (maxColumnsCount > count)
            {
                maxColumnsCount = count;
            }

            return Enumerable.Range(1, maxColumnsCount)
                             .SelectMany(columnsCount => GetSplitsByColumnsCount(count, columnsCount));
        }

        private static IEnumerable<IEnumerable<int>> GetSplitsByColumnsCount(int count, int columnsCount)
        {
            if (columnsCount == 1)
            {
                return Enumerable.Repeat(Enumerable.Repeat(count, 1), 1);
            }

            return Enumerable.Range(1, count - columnsCount + 1)
                             .Select(s0 => count - columnsCount - s0 + 2)
                             .SelectMany(s0 => GetSplitsByColumnsCount(count - s0, columnsCount - 1)
                                                   .Select(s => GetSplit(s0, s)));
        }

        private static IEnumerable<int> GetSplit(int s0, IEnumerable<int> s)
        {
            return Enumerable.Repeat(s0, 1).Concat(s);
        }

        private static T[] Slice<T>(this T[] original, int skip, int take)
        {
            var slice = new T[take];

            Array.Copy(original, skip, slice, 0, take);

            return slice;
        }

        private static IEnumerable<T[]> Slices<T>(this T[] original, IEnumerable<int> split)
        {
            var skip = 0;

            foreach (var sliceSize in split)
            {
                yield return original.Slice(skip, sliceSize);

                skip += sliceSize;
            }
        }

        private static int GetSize(this CatalogRubricModel rubric)
        {
            return rubric.SubRubrics != null ? rubric.SubRubrics.Length + 1 : 1;
        }
    }
}
