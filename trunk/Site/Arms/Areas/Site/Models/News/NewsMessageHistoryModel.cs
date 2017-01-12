using System;
using Vtb24.Site.Content.News.Models;
using Vtb24.Site.Content.Snapshots.Models;

namespace Vtb24.Arms.Site.Models.News
{
    public class NewsMessageHistoryModel
    {
        public string SnapshotId { get; set; }

        public long EntityId { get; set; }

        public DateTime CreationDate { get; set; }

        public string Author { get; set; }

        public bool IsLastVersion { get; set; }

        public static NewsMessageHistoryModel Map(Snapshot<NewsMessage> original)
        {
            return new NewsMessageHistoryModel
            {
                Author = original.Author,
                SnapshotId = original.Id,
                EntityId = original.Entity.Id,
                CreationDate = original.CreationDate,
                IsLastVersion = false
            };
        }
    }
}
