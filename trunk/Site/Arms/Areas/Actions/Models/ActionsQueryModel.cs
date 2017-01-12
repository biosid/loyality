using System;
using Vtb24.Arms.Helpers;

namespace Vtb24.Arms.Actions.Models
{
    public class ActionsQueryModel
    {
        // ReSharper disable InconsistentNaming

        public long? mechanic { get; set; }

        public DateTime? from { get; set; }

        public DateTime? to { get; set; }

        public string term { get; set; }

        public Statuses? status { get; set; }

        public Types? type { get; set; }

        public int? page { get; set; }

        // ReSharper restore InconsistentNaming

        public string ToQuery()
        {
            return QueryHelper.ToQuery(this);
        }

        public ActionsQueryModel MixQuery(string query, bool overwrite = false)
        {
            return QueryHelper.MixQueryTo(this, query, overwrite);
        }
    }
}
