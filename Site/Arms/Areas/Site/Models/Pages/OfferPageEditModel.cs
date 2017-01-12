using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vtb24.Site.Content.History.Models;
using Vtb24.Site.Content.Pages.Models;

namespace Vtb24.Arms.Site.Models.Pages
{
    public class OfferPageEditModel
    {
        public bool IsNewPage { get; set; }

        public int PartnerId { get; set; }

        public string PartnerName { get; set; }

        public SelectListItem[] Partners { get; set; }

        public Guid CurrentVersionId { get; set; }

        public Guid ThisVersionId { get; set; }

        public PageData Data { get; set; }

        public Snapshot[] Versions { get; set; }

        public string Query { get; set; }

        public OfferPagesQueryModel QueryModel
        {
            get
            {
                return string.IsNullOrWhiteSpace(Query)
                           ? new OfferPagesQueryModel()
                           : new OfferPagesQueryModel().MixQuery(Query);
            }
        }
    }
}