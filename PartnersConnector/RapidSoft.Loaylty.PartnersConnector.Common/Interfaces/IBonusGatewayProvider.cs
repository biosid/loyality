namespace RapidSoft.Loaylty.PartnersConnector.Interfaces
{
    using BonusGateway.BonusGateway;

    public interface IBonusGatewayProvider
    {
        RollbackPointsResponse CancelPayment(int partnerId, string orderId);
    }
}