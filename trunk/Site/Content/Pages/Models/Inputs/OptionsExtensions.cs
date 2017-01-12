using System.Data.Entity;
using System.Linq;

namespace Vtb24.Site.Content.Pages.Models.Inputs
{
    internal static class OptionsExtensions
    {
        public static IQueryable<Page> ApplyOptions(this IQueryable<Page> query, GetPlainPagesOptions options, bool applySortOrder = true)
        {
            if (options.IsBuiltin.HasValue)
                query = query.Where(p => p.IsBuiltin == options.IsBuiltin.Value);

            if (options.Status.HasValue)
                query = query.Where(p => p.Status == options.Status.Value);

            if (options.LoadFullHistory)
                query = query.Include(p => p.History.Versions);

            if (!applySortOrder)
                return query;

            switch (options.SortOrder)
            {
                case PagesSortOrder.UrlAsc:
                    return query.OrderBy(p => p.History.CurrentVersion.Data.Url);

                default:
                    return query;
            }
        }
    }
}
