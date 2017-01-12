using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vtb24.Arms.Areas.Catalog.Models.Delivery
{
    public class DropdownPartnerRowModel
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public PartnerType Type { get; set; }

        public bool IsSelected { get; set; }
    }
}