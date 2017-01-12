using Vtb24.Arms.Helpers;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public class GiftsQueryModel
    {

        // ReSharper disable InconsistentNaming

        public string term { get; set; }

        public int? supplier { get; set; }

        public int? category { get; set; }

        public Sortings? sort { get; set; }

        public Moderations? moderation { get; set; }

        public bool recommended_only { get; set; }

        public int? page { get; set; }
        
        // ReSharper restore InconsistentNaming

        public string ToQuery()
        {
            return QueryHelper.ToQuery(this);
        }

        public GiftsQueryModel MixQuery(string query, bool overwrite = false)
        {
            return QueryHelper.MixQueryTo(this, query, overwrite);
        }
    }
}
