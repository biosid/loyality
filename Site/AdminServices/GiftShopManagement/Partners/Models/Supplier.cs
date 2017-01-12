namespace Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models
{
    public class Supplier : Partner
    {
        public SupplierTrustLevel TrustLevel { get; set; }

        public SupplierType Type { get; set; }

        public int? CarrierId { get; set; }
    }
}
