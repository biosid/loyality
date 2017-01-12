using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vtb24.Site.Services.GeoService.Models;

namespace Vtb24.Arms.Areas.Catalog.Models.Delivery
{
    public class AutocompleteSuggestionContextModel
    {
        // ReSharper disable InconsistentNaming

        public string selectionFullLabel { get; set; }

        public string selectionLabel { get; set; }

        public string selectionValue { get; set; }

        public string regionValue { get; set; }

        public string regionLabel { get; set; }

        public string districtValue { get; set; }

        public string districtLabel { get; set; }

        public string type { get; set; }

        // ReSharper restore InconsistentNaming
    }
}