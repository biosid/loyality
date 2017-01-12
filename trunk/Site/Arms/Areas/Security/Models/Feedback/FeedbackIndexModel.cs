using Vtb24.Arms.Helpers;

namespace Vtb24.Arms.Security.Models.Feedback
{
    public class FeedbackIndexModel
    {
        public FeedbackIndexFiltersModel Filters { get; set; }

        public FeedbackIndexThreadModel[] Threads { get; set; }

        public int PagesCount { get; set; }

        public string SerializeFilters()
        {
            return QueryHelper.ToQuery(Filters);
        }

        public static object DeserializeFilters(string serialized)
        {
            return string.IsNullOrWhiteSpace(serialized) ? null : QueryHelper.MixQueryTo(new FeedbackIndexFiltersModel(), serialized);
        }
    }
}