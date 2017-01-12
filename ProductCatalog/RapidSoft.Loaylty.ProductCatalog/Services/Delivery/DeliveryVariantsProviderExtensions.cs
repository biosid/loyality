namespace RapidSoft.Loaylty.ProductCatalog.Services.Delivery
{
	using API.Entities;
	using API.InputParameters;
	using API.OutputResults;

	public static class DeliveryVariantsProviderExtensions
	{
		public static GetDeliveryVariantsResult GetSiteDeliveryVariants(this IDeliveryVariantsProvider provider, int partnerId, Location location, OrderItem orderItem, string clientId)
		{
			return provider.GetSiteDeliveryVariants(partnerId, location, new[] { orderItem }, clientId);
		}

		public static PartnerDeliveryVariants GetPartnerDeliveryVariants(this IDeliveryVariantsProvider provider, int partnerId, Location location, OrderItem orderItem, string clientId)
		{
			return provider.GetPartnerDeliveryVariants(partnerId, location, new[] { orderItem }, clientId);
		}		
	}
}
