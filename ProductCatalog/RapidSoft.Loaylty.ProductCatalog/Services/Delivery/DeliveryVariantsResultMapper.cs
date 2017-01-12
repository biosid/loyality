namespace RapidSoft.Loaylty.ProductCatalog.Services.Delivery
{
	using API.Entities;
	using API.OutputResults;
	using AutoMapper;

	public static class DeliveryVariantsResultMapper
	{
		static DeliveryVariantsResultMapper()
		{
			Mapper.CreateMap<PartnersConnector.WsClients.PartnersOrderManagementService.GetDeliveryVariantsResult, GetDeliveryVariantsResult>();
			Mapper.CreateMap<PartnersConnector.WsClients.PartnersOrderManagementService.VariantsLocation, VariantsLocation>();
			Mapper.CreateMap<PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryGroup, DeliveryGroup>();
			Mapper.CreateMap<PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryVariant, DeliveryVariant>()
				.ForMember(dest => dest.DeliveryType, opt => opt.MapFrom(src => GetDeliveryVariant(src)));
			Mapper.CreateMap<PartnersConnector.WsClients.PartnersOrderManagementService.PickupPoint, PickupVariant>()
				.ForMember(dest => dest.PickupPoint, opt => opt.MapFrom(src => src)).ForMember(dest => dest.DeliveryCost, opt => opt.MapFrom(src => src.DeliveryCost));
			Mapper.CreateMap<PartnersConnector.WsClients.PartnersOrderManagementService.PickupPoint, PickupPoint>();
		}

		public static GetDeliveryVariantsResult Map(PartnerDeliveryVariants from)
		{
			var to = Mapper.Map<PartnersConnector.WsClients.PartnersOrderManagementService.GetDeliveryVariantsResult, GetDeliveryVariantsResult>(from.Variants);
			to.ResultCode = ResultCodes.SUCCESS;
			return to;
		}

		private static DeliveryTypes GetDeliveryVariant(PartnersConnector.WsClients.PartnersOrderManagementService.DeliveryVariant src)
		{
			if (src.PickupPoints != null && src.PickupPoints.Length > 0)
			{
				return DeliveryTypes.Pickup;
			}

			return DeliveryTypes.Delivery;
		}
	}
}
