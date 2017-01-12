using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vtb24.Arms.Areas.Catalog.Models.Delivery
{
    public class HistoryModel
    {
        public DropdownPartnerModel Dropdown { get; set; }

        public PartnerLocationHistoryModel[] BindingsHistory { get; set; }

        public string BackUrl { get; set; }

        public string Title { get; set; }

        public int TotalPages { get; set; }

        // ReSharper disable InconsistentNaming

        public int? page { get; set; }

        // ReSharper restore InconsistentNaming
    }
}