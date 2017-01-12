namespace RapidSoft.Loaylty.ProductCatalog.Services.Delivery
{
    using System.Collections.Generic;
    using System.Linq;
    using API.OutputResults;
    using PartnersConnector.WsClients.PartnersOrderManagementService;
    using DeliveryTypes = API.Entities.DeliveryTypes;
    using GetDeliveryVariantsResult = API.OutputResults.GetDeliveryVariantsResult;
    using Location = API.InputParameters.Location;
    using OrderItem = API.Entities.OrderItem;

    public class DeliveryVariantsProviderWithEmail : IDeliveryVariantsProvider
    {
        private readonly IDeliveryVariantsProvider deliveryVariantsProviderWithoutEmail;

        public DeliveryVariantsProviderWithEmail(IDeliveryVariantsProvider deliveryVariantsProviderWithoutEmail)
        {
            this.deliveryVariantsProviderWithoutEmail = deliveryVariantsProviderWithoutEmail;
        }

        public GetDeliveryVariantsResult GetSiteDeliveryVariants(int partnerId, Location location, OrderItem[] orderItems, string clientId)
        {
            var res = GetPartnerDeliveryVariants(partnerId, location, orderItems, clientId);

            var siteRes = DeliveryVariantsResultMapper.Map(res);

            foreach (var deliveryVariant in siteRes.DeliveryGroups.SelectMany(g => g.DeliveryVariants))
            {
                if (deliveryVariant.DeliveryVariantName == Consts.EmailDeliveryVariantName && deliveryVariant.DeliveryType == DeliveryTypes.Delivery)
                {
                    deliveryVariant.DeliveryType = DeliveryTypes.Email;
                }
            }

            return siteRes;
        }

        public PartnerDeliveryVariants GetPartnerDeliveryVariants(int partnerId, Location location, OrderItem[] orderItems, string clientId)
        {
            PartnerDeliveryVariants result;

            if (EmailDeliveryVariantShouldBeOnly(orderItems))
            {
                result = new PartnerDeliveryVariants
                {
                    Variants = new PartnersConnector.WsClients.PartnersOrderManagementService.GetDeliveryVariantsResult
                    {
                        ResultCode = ResultCodes.SUCCESS,
                        Success = true
                    }
                };

                AddEmailDeliveryVariantToResult(result.Variants);
            }
            else
            {
                result = deliveryVariantsProviderWithoutEmail.GetPartnerDeliveryVariants(partnerId, location, orderItems, clientId);

                if (EmailDeliveryVariantShouldBeAdded(orderItems))
                {
                    AddEmailDeliveryVariantToResult(result.Variants);
                }
            }

            return result;
        }

        private bool EmailDeliveryVariantShouldBeOnly(IEnumerable<OrderItem> orderItems)
        {
            if (orderItems.All(i => i.Product.IsDeliveredByEmail))
            {
                return true;
            }

            return false;
        }

        private bool EmailDeliveryVariantShouldBeAdded(IEnumerable<OrderItem> orderItems)
        {
            if (orderItems.Any(i => i.Product.IsDeliveredByEmail))
            {
                return true;
            }

            return false;
        }

        private void AddEmailDeliveryVariantToResult(PartnersConnector.WsClients.PartnersOrderManagementService.GetDeliveryVariantsResult getDeliveryVariantsResult)
        {
            var groups = getDeliveryVariantsResult.DeliveryGroups != null
                ? getDeliveryVariantsResult.DeliveryGroups.ToList()
                : new List<DeliveryGroup>();

            groups.Add(new DeliveryGroup
            {
                DeliveryVariants = new[]
                {
                    new DeliveryVariant
                    {
                        DeliveryVariantName = Consts.EmailDeliveryVariantName,
                        DeliveryCost = 0
                    }
                }
            });

            getDeliveryVariantsResult.DeliveryGroups = groups.ToArray();
        }
    }
}