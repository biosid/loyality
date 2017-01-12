namespace RapidSoft.Loaylty.ProductCatalog.Services.Delivery
{
	using System.Collections.Generic;
	using System.Linq;
	using API.Entities;
	using API.OutputResults;
	using DataSources.Interfaces;
	using Extensions;

	public class DeliveryVariantsProviderFactory
	{
		private readonly IPartnerRepository partnerRepository;
		private readonly IDeliveryVariantsProvider defaultProvider;

		public DeliveryVariantsProviderFactory(IPartnerRepository partnerRepository, IDeliveryVariantsProvider defaultProvider)
		{
			this.partnerRepository = partnerRepository;
			this.defaultProvider = defaultProvider;
		}

		public IDeliveryVariantsProvider CreateDeliveryVariantsProvider(int partnerId, IEnumerable<OrderItem> orderItems)
		{
			orderItems.ThrowIfNull("orderItems");

			var partner = partnerRepository.GetById(partnerId);

			if (partner == null)
			{
				throw new OperationException(ResultCodes.PARTNER_NOT_FOUND, "Не найден партнер с илентификатором " + partnerId);
			}

			if (partner.Type == PartnerType.Direct && orderItems.Any(i => i.Product != null && i.Product.IsDeliveredByEmail))
			{
				return new DeliveryVariantsProviderWithEmail(defaultProvider);
			}

			return defaultProvider;
		}
	}
}
