namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using API.Entities;

    public interface IOrdersHistoryRepository
    {
        OrderHistoryPage GetOrderHistory(
            int orderId,
            int countToSkip,
            int countToTake,
            bool calcTotalCount);
    }
}