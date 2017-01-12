using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models
{
    public class PartnerLocation
    {
        public int Id { get; set; }

        public DateTime CreationDateTime { get; set; }

        public string KladrCode { get; set; }

        public string ExternalLocationId { get; set; }

        public string LocationName { get; set; }

        public int PartnerId { get; set; }

        public PartnerLocationStatus Status { get; set; }

        public DateTime? LastUpdateDateTime { get; set; }

        public string LastUpdateUserId { get; set; }
    }
}
