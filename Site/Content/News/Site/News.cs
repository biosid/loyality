using System;
using System.Linq;
using Vtb24.Site.Content.DataAccess;
using Vtb24.Site.Content.Models;
using Vtb24.Site.Content.News.Models;
using Vtb24.Site.Content.News.Models.Outputs;

namespace Vtb24.Site.Content.News.Site
{
    public class News : INews
    {
        public GetNewsMessagesResult GetNewsMessages(string[] segments, PagingSettings paging)
        {
            using (var service = new ContentServiceDbContext())
            {
                var query = service.NewsMessages.Where(m => m.IsPublished);

                segments = segments ?? new string[] {};

                query = query.Where(m => segments.Contains(m.Segment) || string.IsNullOrEmpty(m.Segment));

                query = query.OrderBy(m => m.Priority).ThenByDescending(m => m.Segment).ThenBy(m => m.StartDate);

                query = query.Where(m => m.StartDate < DateTime.Now).Where(m => !m.EndDate.HasValue || m.EndDate > DateTime.Now);

                var totalCount = query.Count();

                query = query.Skip(paging.Skip).Take(paging.Take);

                var result = query.ToArray();

                return new GetNewsMessagesResult(result, totalCount, paging);
            }
        }

        public NewsMessage GetById(long id)
        {
            using (var service = new ContentServiceDbContext())
            {
                return service.NewsMessages.Find(id);
            }
        }
    }
}
