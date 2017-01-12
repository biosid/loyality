using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vtb24.Site.Content.Pages.Models;

namespace Vtb24.Arms.Site.Models.Pages
{
    public class OfferPageModel
    {
        public Guid Id { get; set; }

        public PageStatus Status { get; set; }

        public string PartnerName { get; set; }

        public string Url { get; set; }

        public string Author { get; set; }

        public DateTime WhenModified { get; set; }

        public int PartnerId { get; set; }

        public static OfferPageModel Map(Page page, int partnerId)
        {
            return new OfferPageModel
            {
                Id = page.Id,
                Status = page.Status,
                PartnerName = page.CurrentVersion.Data.Title,
                Url = page.CurrentVersion.Data.Url,
                Author = page.CurrentVersion.Author,
                WhenModified = page.CurrentVersion.When,
                PartnerId = partnerId
            };
        }
    }
}