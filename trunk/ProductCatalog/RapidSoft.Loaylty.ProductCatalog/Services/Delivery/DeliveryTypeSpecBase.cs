namespace RapidSoft.Loaylty.ProductCatalog.Services.Delivery
{
    public class DeliveryTypeSpecBase
    {
        protected CurrierDeliveryPrice BuildDeliveryPriceResult(int? carrierId, decimal deliveryRur)
        {
            return new CurrierDeliveryPrice()
            {
                CarrierId = carrierId,
                PriceDeliveryRur = deliveryRur
            };
        }
    }
}