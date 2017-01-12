using System;
using Vtb24.Arms.Helpers;

namespace Vtb24.Arms.Security.Models.Users
{
    public class UserOrdersQueryModel
    {
        // ReSharper disable InconsistentNaming

        public string login { get; set; }

        public DateTime? from { get; set; }

        public DateTime? to { get; set; }

        public OrdersKind kind { get; set; }

        public int? page { get; set; }

        // ReSharper restore InconsistentNaming

        public string ToQuery()
        {
            return QueryHelper.ToQuery(this);
        }

        public UserOrdersQueryModel MixQuery(string query, bool overwrite = false)
        {
            return QueryHelper.MixQueryTo(this, query, overwrite);
        }
    }
}
