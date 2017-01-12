using System.Collections.Generic;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models
{
    public abstract class Partner
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PartnerStatus Status { get; set; }

        public Dictionary<string, string> Settings { get; set; }
    }
}
