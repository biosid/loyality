using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models.Inputs
{
    public class GetPartnerLocationsOptions
    {
        public int PartnerId { get; set; }

        public PartnerLocationStatus[] Statuses { get; set; }

        public string SearchTerm { get; set; }
    }
}
