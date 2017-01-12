using System;

namespace Vtb24.Site.Content.News.Management.Models.Inputs
{
    public class GetNewsMessagesFilter
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public bool IncludeUnpublished { get; set; }

        public string Keyword { get; set; }
    }
}
