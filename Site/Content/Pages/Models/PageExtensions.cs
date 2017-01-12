using System.Data.Entity;

namespace Vtb24.Site.Content.Pages.Models
{
    public static class PageExtensions
    {
        public static Page IncludeHistory(this Page page, DbContext context)
        {
            context.Entry(page).Reference(p => p.History).Load();
            return page;
        }
    }
}
