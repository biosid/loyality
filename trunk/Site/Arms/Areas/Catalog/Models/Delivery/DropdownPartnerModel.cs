using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vtb24.Arms.Areas.Catalog.Models.Delivery
{
    public class DropdownPartnerModel
    {
        public DropdownPartnerRowModel[] Carriers { get; set; }

        public DropdownPartnerRowModel[] Suppliers { get; set; }

        public int SelectedPartnerId { get; set; }
    }
}