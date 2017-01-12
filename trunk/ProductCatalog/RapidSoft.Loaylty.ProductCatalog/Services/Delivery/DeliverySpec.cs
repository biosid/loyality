namespace RapidSoft.Loaylty.ProductCatalog.Services.Delivery
{
	using API.Entities;
	using API.InputParameters;
	using API.OutputResults;
	using DataSources.Interfaces;
	using DataSources.Repositories;
	using Extensions;
	using Interfaces;

	public class DeliverySpec : DeliverySpecBase, IDeliveryTypeSpec
    {
        private readonly IGeoPointProvider geoPointProvider;
        private readonly PartnerDeliveryVariants partnerDeliveryVariants;
        private readonly IPartnerRepository partnerRepository;

        public DeliverySpec(
            PartnerDeliveryVariants partnerDeliveryVariants, 
            IGeoPointProvider geoPointProvider = null,
            IPartnerRepository partnerRepository = null)
            : base(partnerDeliveryVariants)
        {
            partnerDeliveryVariants.ThrowIfNull("partnerDeliveryVariants");

            this.partnerRepository = partnerRepository ?? new PartnerRepository();
            this.partnerDeliveryVariants = partnerDeliveryVariants;
            this.geoPointProvider = geoPointProvider ?? new GeoPointProvider();
        }

        public void FillDeliveryAddress(DeliveryAddress deliveryAddress, string kladrCode)
        {
            Utils.CheckArgument(o => !string.IsNullOrEmpty(o) && o.Length != 13, kladrCode, "kladrCode");

            if (string.IsNullOrWhiteSpace(kladrCode))
            {
                const string Error =
                    "Для доставки должен быть указан КЛАДР";
                throw new OperationException(ResultCodes.INVALID_DELIVARY_KLADR, Error);
            }

            var kladrAddress = geoPointProvider.GetAddressByKladrCode(kladrCode);

            deliveryAddress.ResetTitles(kladrAddress);
            deliveryAddress.AddressText = deliveryAddress.BuildAddressText(kladrAddress);
        }

        public DeliveryInfo BuildDeliveryInfo(DeliveryDto delivery, int partnerId)
        {
            delivery.ThrowIfNull("delivery");

            var address = GetAddress(delivery);
            
            var variantsLocation = GetVariantsLocation();

            var variant = GetDeliveryVariant(delivery.ExternalDeliveryVariantId);
            
            var deliveryInfo = new DeliveryInfo()
            {
                DeliveryType = delivery.DeliveryType,
                DeliveryVariantsLocation = new VariantsLocation()
                {
                    ExternalLocationId = variantsLocation.ExternalLocationId,
                    KladrCode = variantsLocation.KladrCode,
                    PostCode = variantsLocation.PostCode,
                    LocationName = variantsLocation.LocationName
                },                
                ExternalDeliveryVariantId = variant.ExternalDeliveryVariantId,
                DeliveryVariantName = variant.DeliveryVariantName,
                DeliveryVariantDescription = variant.Description,
                Address = address,
                Contact = delivery.Contact,
                Comment = delivery.Comment
            };

            var partner = partnerRepository.GetActivePartner(partnerId);

            if (partner.Settings.GetDeliveryVariantsSupported())
            {
                deliveryInfo.Address.AddressText = deliveryInfo.Address.BuildAddressText();
            }
            else
            {
                delivery.DeliveryVariantLocation.ThrowIfNull("delivery.DeliveryVariantLocation");

                FillDeliveryAddress(deliveryInfo.Address, delivery.DeliveryVariantLocation.KladrCode);

                // Хак для директов, заполняем LocationName полным текстом адреса полученным из кладр
                if (string.IsNullOrEmpty(deliveryInfo.DeliveryVariantsLocation.LocationName))
                {
                    deliveryInfo.DeliveryVariantsLocation.LocationName = deliveryInfo.Address.AddressText;
                }
            }

            return deliveryInfo;
        }

        public decimal GetDeliveryCost(string externalVariantId, string externalPickupPointId)
        {
            externalVariantId.ThrowIfNull("externalVariantId");

            var variant = GetDeliveryVariant(externalVariantId);
            return variant.DeliveryCost;
        }

        private static DeliveryAddress GetAddress(DeliveryDto delivery)
        {
            delivery.Address.ThrowIfNull("delivery.Address");
            Utils.CheckArgument(string.IsNullOrEmpty, delivery.Address.StreetTitle, "StreetTitle");
            Utils.CheckArgument(string.IsNullOrEmpty, delivery.Address.House, "House");

            return delivery.Address;
        }
    }
}