using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vtb24.Arms.Helpers;

namespace Vtb24.Arms.Areas.Catalog.Models.Delivery
{
    public class DeliveryQueryModel
    {
        // ReSharper disable InconsistentNaming

        public bool hidebinded { get; set; }

        public int? partnerId { get; set; }

        public int? page { get; set; }

        public string term { get; set; }

        // ReSharper restore InconsistentNaming

        public string ToQuery()
        {
            return QueryHelper.ToQuery(this);
        }

        public DeliveryQueryModel MixQuery(string query, bool overwrite = false)
        {
            return QueryHelper.MixQueryTo(this, query, overwrite);
        }
    }
}