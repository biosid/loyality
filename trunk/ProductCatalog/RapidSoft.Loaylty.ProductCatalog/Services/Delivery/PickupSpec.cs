namespace RapidSoft.Loaylty.ProductCatalog.Services.Delivery
{
	using System;
	using API.Entities;
	using API.InputParameters;
	using DataSources.Interfaces;
	using DataSources.Repositories;
	using Extensions;
	using Interfaces;

	public class PickupSpec : DeliverySpecBase, IDeliveryTypeSpec
    {
        private readonly PartnerDeliveryVariants partnerDeliveryVariants;
        private readonly IPartnerRepository partnerRepository;

        public PickupSpec(PartnerDeliveryVariants partnerDeliveryVariants, IPartnerRepository partnerRepository = null) : base(partnerDeliveryVariants)
        {
            partnerDeliveryVariants.ThrowIfNull("partnerDeliveryVariants");

            this.partnerRepository = partnerRepository ?? new PartnerRepository();            
            this.partnerDeliveryVariants = partnerDeliveryVariants;
        }

        public DeliveryInfo BuildDeliveryInfo(DeliveryDto delivery, int partnerId)
        {
            delivery.ThrowIfNull("delivery");                       
            
            delivery.PickupPoint.ThrowIfNull("delivery.PickupPoint");

            var variant = GetDeliveryVariant(delivery.ExternalDeliveryVariantId);
            var point = GetPickUpPoint(delivery.ExternalDeliveryVariantId, delivery.PickupPoint.ExternalPickupPointId);

            var variantsLocation = GetVariantsLocation();

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
                ExternalDeliveryVariantId = point.ExternalDeliveryVariantId,
                DeliveryVariantName = variant.DeliveryVariantName,
                DeliveryVariantDescription = variant.Description,
                PickupPoint = new API.Entities.PickupPoint()
                {
                    Name = point.Name,
                    ExternalPickupPointId = point.ExternalPickupPointId,
                    ExternalDeliveryVariantId = point.ExternalDeliveryVariantId,
                    Address = point.Address,
                    Phones = point.Phones,
                    OperatingHours = point.OperatingHours,
                    Description = point.Description
                },
                Contact = delivery.Contact,
                Comment = delivery.Comment
            };

            var partner = partnerRepository.GetActivePartner(partnerId);

            if (partner.Settings.GetDeliveryVariantsSupported())
            {
                // deliveryInfo.Address.AddressText = deliveryInfo.Address.BuildAddressText();
            }
            else
            {
                throw new Exception(string.Format("Partner {0} not support GetDeliveryVariants. DeliveryTypes.Pickup denied", partnerId));
            }

            return deliveryInfo;
        }

        public decimal GetDeliveryCost(string externalVariantId, string externalPickupPointId)
        {
            externalVariantId.ThrowIfNull("externalVariantId");
            externalPickupPointId.ThrowIfNull("externalPickupPointId");

            var pickUpPoint = GetPickUpPoint(externalVariantId, externalPickupPointId);
            return pickUpPoint.DeliveryCost;
        }
    }
}