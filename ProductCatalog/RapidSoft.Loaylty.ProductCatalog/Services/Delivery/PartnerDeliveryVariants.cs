namespace RapidSoft.Loaylty.ProductCatalog.Services.Delivery
{
	using PartnersConnector.WsClients.PartnersOrderManagementService;

	public class PartnerDeliveryVariants
    {
        public GetDeliveryVariantsResult Variants
        {
            get;
            set;
        }

        public int? CarrierId
        {
            get;
            set;
        }
    }
}