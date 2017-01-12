namespace RapidSoft.Loaylty.ProductCatalog.Interfaces
{
    using RapidSoft.VTB24.BankConnector.WsClients.PaymentService;

    public interface IAdvancePaymentProvider
    {
        SimpleBankConnectorResponse ConfirmPayment(int orderId);

        SimpleBankConnectorResponse CancelPayment(int orderId);
    }
}
