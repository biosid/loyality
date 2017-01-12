using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vtb24.Arms.Helpers;

namespace Vtb24.Arms.Site.Models.Pages
{
    public class OfferPagesQueryModel
    {
        // ReSharper disable InconsistentNaming

        public int? page { get; set; }

        // ReSharper restore InconsistentNaming

        public string ToQuery()
        {
            return QueryHelper.ToQuery(this);
        }

        public OfferPagesQueryModel MixQuery(string query, bool overwrite = false)
        {
            return QueryHelper.MixQueryTo(this, query, overwrite);
        }
    }
}