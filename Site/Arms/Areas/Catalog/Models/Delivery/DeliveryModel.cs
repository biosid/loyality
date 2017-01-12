using System.Collections.Generic;
using System.Web.Mvc;
using Vtb24.Arms.Areas.Catalog.Models.Delivery;

namespace Vtb24.Arms.Catalog.Models.Delivery
{
    public class DeliveryModel
    {
        public DropdownPartnerModel Dropdown { get; set; }

        public PartnerLocationModel[] Locations { get; set; }

        public string ImportUrl { get; set; }

        public string HistoryUrl { get; set; }

        public UpdateBindingModel UpdateBinding { get; set; }

        public DeliveryQueryModel Query { get; set; }

        public int TotalPages { get; set; }    
    }
}
