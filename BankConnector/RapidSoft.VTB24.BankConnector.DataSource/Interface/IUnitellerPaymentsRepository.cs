using RapidSoft.VTB24.BankConnector.DataModels;

namespace RapidSoft.VTB24.BankConnector.DataSource.Interface
{
    public interface IUnitellerPaymentsRepository
    {
        UnitellerPayment GetByOrderId(int orderId);

        void SaveRequest(int orderId, string shopId);
    }
}
