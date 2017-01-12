using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vtb24.Site.Content.Pages.Models;

namespace Vtb24.Arms.Site.Models.Pages
{
    public class ChangeOfferPageStatusModel
    {
        public int[] PartnerIds { get; set; }

        public PageStatus Status { get; set; }
    }
}