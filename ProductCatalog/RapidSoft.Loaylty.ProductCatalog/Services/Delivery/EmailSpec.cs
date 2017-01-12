namespace RapidSoft.Loaylty.ProductCatalog.Services.Delivery
{
    using API.Entities;
    using API.InputParameters;
    using Extensions;
    using Interfaces;

    public class EmailSpec : IDeliveryTypeSpec
    {
        public DeliveryInfo BuildDeliveryInfo(DeliveryDto delivery, int partnerId)
        {
            delivery.ThrowIfNull("delivery");

            var deliveryInfo = new DeliveryInfo
            {
                DeliveryType = delivery.DeliveryType,
                DeliveryVariantName = Consts.EmailDeliveryVariantName,
                Contact = delivery.Contact,
                Comment = delivery.Comment
            };

            return deliveryInfo;
        }

        public decimal GetDeliveryCost(string externalVariantId, string externalPickupPointId)
        {
            // Стоимость доставки по e-mail (независимо от матрицы стоимости доставки поставщика) равна 0 рублям\бонусам
            return 0M;
        }
    }
}