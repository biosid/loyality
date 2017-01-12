using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using Vtb24.Site.Content.DataAccess;
using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.Pages.Models;
using Vtb24.Site.Content.Pages.Models.Configuration;
using Vtb24.Site.Content.Pages.Models.Inputs;
using Vtb24.Site.Content.Pages.Models.Outputs;

namespace Vtb24.Site.Content.Pages
{
    public class PagesManagement : IPagesManagement
    {
        public GetAllPagesResult GetAllPlainPages(GetPlainPagesOptions options, PagingSettings paging)
        {
            using (var context = new ContentServiceDbContext())
            {
                var query = context.Pages
                                   .AsNoTracking()
                                   .Where(p => p.Type == PageType.Plain)
                                   .Include(p => p.History.CurrentVersion)
                                   .ApplyOptions(options);

                var pages = query.Skip(paging.Skip)
                                 .Take(paging.Take)
                                 .ToArray();

                var totalCount = query.Count();

                return new GetAllPagesResult(pages, totalCount, paging);
            }
        }

        public Page GetPlainPageById(Guid id, GetPlainPagesOptions options)
        {
            using (var context = new ContentServiceDbContext())
            {
                var query = context.Pages
                                   .AsNoTracking()
                                   .Where(p => p.Id == id && 
                                               p.Type == PageType.Plain)
                                   .Include(p => p.History.CurrentVersion)
                                   .ApplyOptions(options, false);

                return query.FirstOrDefault();
            }
        }

        public bool HasPageWithUrl(string url, GetPlainPagesOptions options = new GetPlainPagesOptions())
        {
            using (var context = new ContentServiceDbContext())
            {
                var query = CreatePageByUrlQuery(context, url, options);
                return query.Any();
            }
        }

        public Page GetPageByUrl(string url, GetPlainPagesOptions options)
        {
            using (var context = new ContentServiceDbContext())
            {
                var query = CreatePageByUrlQuery(context, url, options);
                return query.FirstOrDefault();
            }
        }

        public Page GetOfferPageByPartnerId(int partnerId, bool loadFullHistory)
        {
            using (var context = new ContentServiceDbContext())
            {
                var partnerIdStr = partnerId.ToString(CultureInfo.InvariantCulture);

                var query = context.Pages
                                   .AsNoTracking()
                                   .Where(p => p.ExternalId == partnerIdStr &&
                                               p.Type == PageType.Offer)
                                   .Include(p => p.History.CurrentVersion);

                if (loadFullHistory)
                    query = query.Include(p => p.History.Versions);

                return query.FirstOrDefault();
            }
        }

        public bool IsPartnerGotOfferPage(int partnerId)
        {
            using (var context = new ContentServiceDbContext())
            {
                var partnerIdStr = partnerId.ToString(CultureInfo.InvariantCulture);

                var query = context.Pages
                                   .AsNoTracking()
                                   .Any(p => p.Type == PageType.Offer && partnerIdStr == p.ExternalId);

                return query;
            }
        }

        public PageData GetPageVersionById(Guid id)
        {
            using (var context = new ContentServiceDbContext())
            {
                var snapshot = context.Set<PageSnapshot>()
                                      .AsNoTracking()
                                      .FirstOrDefault(s => s.Id == id);

                return snapshot != null
                           ? snapshot.Data
                           : null;
            }
        }

        public void ReloadBuiltinPagesFromConfiguration()
        {
            using (var context = new ContentServiceDbContext())
            {
                var section = (BuiltinPagesConfigSection) ConfigurationManager.GetSection("builtin_pages");

                var configPages = section.PagesCollection
                                         .Cast<BuiltinPageElement>()
                                         .Select(Page.Map)
                                         .ToArray();

                var dbPages = context.Pages
                                     .Where(p => p.IsBuiltin)
                                     .Include(p => p.History.CurrentVersion)
                                     .ToArray();

                dbPages = RemoveBuiltinPages(context, dbPages, configPages).ToArray();

                AddBuiltinPages(context, configPages, dbPages);

                context.SaveChanges();
            }
        }

        private static IEnumerable<Page> RemoveBuiltinPages(ContentServiceDbContext context, IEnumerable<Page> dbPages, Page[] configPages)
        {
            foreach (var dbPage in dbPages)
            {
                var url = dbPage.CurrentVersion.Data.Url;

                if (configPages.Any(configPage => configPage.CurrentVersion.Data.Url == url))
                    yield return dbPage;
                else
                    context.Pages.Remove(dbPage);
            }
        }

        private static void AddBuiltinPages(ContentServiceDbContext context, IEnumerable<Page> configPages, Page[] dbPages)
        {
            foreach (var configPage in configPages)
            {
                var url = configPage.CurrentVersion.Data.Url;

                if (dbPages.Any(dbPage => dbPage.CurrentVersion.Data.Url == url))
                    continue;

                var existingPage = context
                    .Pages
                    .FirstOrDefault(p => p.History.CurrentVersion.Data.Url == url);

                configPage.CurrentVersion.Author = "конфигурация";
                configPage.CurrentVersion.When = DateTime.Now;

                Page page;
                try
                {
                    page = Page.Create(configPage.CurrentVersion, context);
                }
                catch (DbEntityValidationException)
                {
                    continue;
                }

                page.IsBuiltin = true;
                page.Status = PageStatus.Active;
                page.Type = PageType.Plain;

                context.Pages.Add(page);

                if (existingPage != null)
                    context.Pages.Remove(existingPage);
            }
        }

        private static IQueryable<Page> CreatePageByUrlQuery(ContentServiceDbContext context, string url, GetPlainPagesOptions options)
        {
            return context.Pages
                          .AsNoTracking()
                          .Where(p => p.History.CurrentVersion.Data.Url == url)
                          .Include(p => p.History.CurrentVersion)
                          .ApplyOptions(options, false);
        }
    }
}
