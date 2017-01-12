namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using API.Entities;
    using API.InputParameters;

    using Services;

    public interface IDeliveryTypeSpec
    {
        DeliveryInfo BuildDeliveryInfo(DeliveryDto delivery, int partnerId);

        decimal GetDeliveryCost(string externalVariantId, string externalPickupPointId);
    }
}