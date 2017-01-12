using System.Collections.Generic;
using System.Linq;

namespace Rapidsoft.VTB24.Reports.Site.Helpers
{
    public static class PagerHelper
    {
        public static IEnumerable<int> GeneratePages(int page, int total, int items)
        {
            if (total <= 0)
                return Enumerable.Empty<int>();

            if (total <= items)
                return Enumerable.Range(1, total);

            if (items < 5)
                items = 5;

            if (page < items - 2)
                return Enumerable.Range(1, items - 2)
                                 .Concat(new[] { -1, total });

            if (page > total - items + 3)
                return new[] { 1, -1 }
                    .Concat(Enumerable.Range(total - items + 3, items - 2));

            return new[] { 1, -1 }
                .Concat(Enumerable.Range(page - (items - 5) / 2, items - 4))
                .Concat(new[] { -1, total });
        }
    }
}
