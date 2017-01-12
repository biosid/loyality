namespace RapidSoft.Loaylty.ProductCatalog.Services.Delivery
{
	using System;
	using System.Collections.Generic;
	using Extensions;
	using PartnersConnector.WsClients.PartnersOrderManagementService;

	public class DeliverySpecBase
    {
        private readonly PartnerDeliveryVariants partnerDeliveryVariants;

        protected DeliverySpecBase(PartnerDeliveryVariants partnerDeliveryVariants)
        {
            this.partnerDeliveryVariants = partnerDeliveryVariants;
        }

        protected VariantsLocation GetVariantsLocation()
        {
            partnerDeliveryVariants.ThrowIfNull("partnerDeliveryVariants.Variants");
            partnerDeliveryVariants.Variants.ThrowIfNull("partnerDeliveryVariants.Variants.Location");

            return partnerDeliveryVariants.Variants.Location;
        }

        protected DeliveryVariant GetDeliveryVariant(string externalVariantId)
        {
            partnerDeliveryVariants.ThrowIfNull("partnerDeliveryVariants");
            partnerDeliveryVariants.Variants.ThrowIfNull("partnerDeliveryVariants.Variants");
            partnerDeliveryVariants.Variants.DeliveryGroups.ThrowIfNull("partnerDeliveryVariants.Variants.DeliveryGroups");

            var res = new List<DeliveryVariant>();

            foreach (var group in partnerDeliveryVariants.Variants.DeliveryGroups)
            {
                if (group.DeliveryVariants == null)
                {
                    continue;
                }

                foreach (var variant in group.DeliveryVariants)
                {
                    if (variant.ExternalDeliveryVariantId == externalVariantId)
                    {
                        res.Add(variant);
                    }
                }
            }

            if (res.Count < 1)
            {
                throw new Exception(string.Format("DeliveryVariant with externalVariantId:{0} not found", externalVariantId));
            }

            if (res.Count != 1)
            {
                throw new Exception(string.Format("DeliveryVariant with externalVariantId:{0} founded {1} times", externalVariantId, res.Count));
            }

            return res[0];
        }

        protected PickupPoint GetPickUpPoint(string externalVariantId, string externalPickupPointId)
        {
            partnerDeliveryVariants.ThrowIfNull("partnerDeliveryVariants");
            partnerDeliveryVariants.Variants.ThrowIfNull("partnerDeliveryVariants.Variants");
            partnerDeliveryVariants.Variants.DeliveryGroups.ThrowIfNull("partnerDeliveryVariants.Variants.DeliveryGroups");

            var res = new List<PickupPoint>();

            foreach (var group in partnerDeliveryVariants.Variants.DeliveryGroups)
            {
                foreach (var variant in group.DeliveryVariants)
                {
                    foreach (var pickupPoint in variant.PickupPoints)
                    {
                        if (pickupPoint.ExternalDeliveryVariantId == externalVariantId && pickupPoint.ExternalPickupPointId == externalPickupPointId)
                        {
                            res.Add(pickupPoint);
                        }
                    }
                }
            }

            if (res.Count < 1)
            {
                throw new Exception(string.Format("DeliveryVariant with externalVariantId:{0} and externalPickupPointId:{1} not found", externalVariantId, externalPickupPointId));
            }

            if (res.Count != 1)
            {
                throw new Exception(string.Format("DeliveryVariant with externalVariantId:{0} and externalPickupPointId:{1} founded {2} times", externalVariantId, externalPickupPointId, res.Count));
            }

            return res[0];
        }
    }
}