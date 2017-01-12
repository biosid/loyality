namespace RapidSoft.Loaylty.ProductCatalog.Services.Delivery
{
	using API.Entities;
	using API.InputParameters;
	using API.OutputResults;
	using DataSources.Interfaces;
	using DataSources.Repositories;
	using Entities;
	using Interfaces;

	public class DeliveryVariantsProvider : IDeliveryVariantsProvider
    {
        public const string DeliveryMatrixVariantId = "CarrierDelivery";
        private const string DeliveryMatrixVariantName = "Курьерская доставка";
        private readonly IDeliveryRatesRepository ratesRepository;
        private readonly IPartnerRepository partnerRepository;
        private readonly IPartnerConnectorProvider partnerConnectorProvider;

        public DeliveryVariantsProvider(
            IPartnerConnectorProvider partnerConnectorProvider = null,
            IPartnerRepository partnerRepository = null,
            IDeliveryRatesRepository ratesRepository = null)
        {
            this.ratesRepository = ratesRepository ?? new DeliveryRatesRepository();
            this.partnerRepository = partnerRepository ?? new PartnerRepository();
            this.partnerConnectorProvider = partnerConnectorProvider ?? new PartnerConnectorProvider();
        }

		public GetDeliveryVariantsResult GetSiteDeliveryVariants(int partnerId, Location location, OrderItem[] orderItems, string clientId)
		{
			var res = GetPartnerDeliveryVariants(partnerId, location, orderItems, clientId);

			var siteRes = DeliveryVariantsResultMapper.Map(res);

			return siteRes;
		}

		public PartnerDeliveryVariants GetPartnerDeliveryVariants(int partnerId, Location location, OrderItem[] orderItems, string clientId)
        {
            var partner = partnerRepository.GetActivePartner(partnerId);

			if (partner.Settings.GetDeliveryVariantsSupported())
            {
                var result = partnerConnectorProvider.GetDeliveryVariants(location, orderItems, clientId, partnerId);

                return new PartnerDeliveryVariants
                {
                    Variants = result,
                    CarrierId = null
                };
            }

            var rate = GetRate(partnerId, orderItems, location.KladrCode);

            if (rate == null)
            {
                return new PartnerDeliveryVariants
                {
                    Variants = new PartnersConnector.WsClients.PartnersOrderManagementService.GetDeliveryVariantsResult
                    {
                        Success = false,
                        ResultDescription = "Rate доставки не найден, невозможно расчитать доставку по матрице"
                    },
                    CarrierId = null
                };
            }

            return new PartnerDeliveryVariants
            {
                Variants = BuildMatrixDelivery(rate.PriceRur, rate.ExternalLocationId, rate),
                CarrierId = rate.CarrierId,
            };
        }

        private PartnerDeliveryRate GetRate(int partnerId, OrderItem[] orderItems, string kladrCode)
        {
            if (string.IsNullOrEmpty(kladrCode))
            {
                throw new OperationException(ResultCodes.INVALID_PARAMETER_VALUE, "kladrCode не передан, невозможно расчитать доставку по матрице");
            }

            var orderItemWeight = 0;

            foreach (var orderItem in orderItems)
            {
                var weight = orderItem.Product.Weight;

                if (!weight.HasValue)
                {
                    // Если у товара нет веса, берём тариф для веса = 0
                    weight = 0;
                }

                orderItemWeight += weight.Value * orderItem.Amount;
            }

            var rate = ratesRepository.GetMinPriceRate(partnerId, kladrCode, orderItemWeight);

            return rate;
        }

        private PartnersConnector.WsClients.PartnersOrderManagementService.GetDeliveryVariantsResult BuildMatrixDelivery(decimal deliveryCost, string externalLocationId, PartnerDeliveryRate rate)
        {
            return new PartnersConnector.WsClients.PartnersOrderManagementService.GetDeliveryVariantsResult
            {
                Success = true,
                ResultCode = ResultCodes.SUCCESS,
                Location = new PartnersConnector.WsClients.PartnersOrderManagementService.VariantsLocation
                {
                    ExternalLocationId = rate.ExternalLocationId,
                    KladrCode = rate.Kladr
                },
                DeliveryGroups = new[]
                {                    
                    new PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryGroup
                    {
                        GroupName = DeliveryMatrixVariantName,
                        DeliveryVariants = new[]
                        {                            
                            new PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryVariant
                            {
                                ExternalDeliveryVariantId = DeliveryMatrixVariantId,
                                DeliveryCost = deliveryCost,
                                DeliveryVariantName = DeliveryMatrixVariantName
                            }
                        }
                    } 
                }
            };
        }
    }
}