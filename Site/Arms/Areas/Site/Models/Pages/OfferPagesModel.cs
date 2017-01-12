using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vtb24.Arms.Site.Models.Pages
{
    public class OfferPagesModel
    {
        public OfferPageModel[] Pages { get; set; }

        public int TotalPages { get; set; }

        public OfferPagesQueryModel Query { get; set; }
    }
}