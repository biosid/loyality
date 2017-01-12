using System;

namespace Vtb24.Site.Content.News.Management.Models.Inputs
{
    public class UpdateNewsMessageOption
    {
        public string Title { get; set; }

        public int Priority { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsPublished { get; set; }

        public string Picture { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public string Segment { get; set; }
    }
}
