using Vtb24.Arms.Helpers;

namespace Vtb24.Arms.Site.Models.Pages
{
    public class PlainPagesQueryModel
    {
        // ReSharper disable InconsistentNaming

        public bool hide_builtin { get; set; }

        public int? page { get; set; }

        // ReSharper restore InconsistentNaming

        public string ToQuery()
        {
            return QueryHelper.ToQuery(this);
        }

        public PlainPagesQueryModel MixQuery(string query, bool overwrite = false)
        {
            return QueryHelper.MixQueryTo(this, query, overwrite);
        }
    }
}
