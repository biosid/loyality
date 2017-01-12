namespace Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models
{
    public abstract class PartnerInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PartnerStatus Status { get; set; }
    }
}
