namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using RapidSoft.Loaylty.BonusGateway.BonusGateway;

    public interface IBonusGatewayProvider
    {
        RollbackPointsResponse CancelPayment(string paymentRequestId);
    }
}
