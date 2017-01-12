namespace Vtb24.Site.Services.GiftShop.Catalog.Models
{
    public class CatalogPartnerSettings
    {
        public bool IsOnlineDeliveryVariansSupported { get; set; }

        public bool IsOrderRequiresEmail { get; set; }

        public AdvancePaymentSupportMode AdvancePaymentSupport { get; set; }

        public int? MaxAdvanceFraction { get; set; }
    }
}
