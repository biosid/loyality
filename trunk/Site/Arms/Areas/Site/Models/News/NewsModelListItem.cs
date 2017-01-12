using System;
using Vtb24.Site.Content.News.Models;

namespace Vtb24.Arms.Site.Models.News
{
    public class NewsModelListItem
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Priority { get; set; }

        public bool IsActive { get; set; }

        public static NewsModelListItem Map(NewsMessage original)
        {
            return new NewsModelListItem
            {
                Id = original.Id,
                Title = original.Title,
                IsActive = original.IsPublished,
                StartDate = original.StartDate,
                EndDate = original.EndDate,
                Priority = original.Priority
            };
        }
    }
}
