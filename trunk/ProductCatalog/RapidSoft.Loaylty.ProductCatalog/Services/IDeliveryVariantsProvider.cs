namespace RapidSoft.Loaylty.ProductCatalog.Services
{
	using API.OutputResults;
	using Delivery;
	using Location = API.InputParameters.Location;
	using OrderItem = API.Entities.OrderItem;

	public interface IDeliveryVariantsProvider
    {
		GetDeliveryVariantsResult GetSiteDeliveryVariants(int partnerId, Location location, OrderItem[] orderItems, string clientId);

        PartnerDeliveryVariants GetPartnerDeliveryVariants(int partnerId, Location location, OrderItem[] orderItems, string clientId);
    }
}