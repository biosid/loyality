using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models
{
    public class PartnerLocationHistory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PartnerId { get; set; }

        public DateTime DateTime { get; set; }

        public string OldKladrCode { get; set; }

        public string NewKladrCode { get; set; }

        public string UserId { get; set; }

        public PartnerLocationStatus OldStatus { get; set; }
        
        public PartnerLocationStatus NewStatus { get; set; }

        public string OldExternalId { get; set; }

        public string NewExternalId { get; set; }
    }
}
