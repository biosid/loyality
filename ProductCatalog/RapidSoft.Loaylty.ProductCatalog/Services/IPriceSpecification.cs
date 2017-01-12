namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using API.Entities;

    public interface IPriceSpecification
    {
        void FillOrderPrice(Order order, OrderItemPrice[] orderPrices, decimal deliveryCost, decimal bonusDeliveryCost, decimal deliveryAdvance, decimal itemsAdvance);
    }
}