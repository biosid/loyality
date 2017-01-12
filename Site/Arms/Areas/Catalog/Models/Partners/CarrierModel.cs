using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Vtb24.Arms.Helpers;

namespace Vtb24.Arms.Catalog.Models.Partners
{
    public class CarrierModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Status { get; set; }

        public string Description { get; set; }

        public bool Disabled { get; set; }

        public static CarrierModel Map(Carrier carrier)
        {
            return new CarrierModel
            {
                Id = carrier.Id,
                Name = carrier.Name,
                Status = carrier.Status.Map().EnumDescription(),
                Description = carrier.Description,
                Disabled = carrier.Status == PartnerStatus.Disabled
            };
        }
    }
}
