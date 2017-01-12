namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Outputs
{
    public class UnitellerPaymentInfo
    {
        public string OrderId { get; set; }

        public string BillNumber { get; set; }

        public bool IsAuthorized { get; set; }
    }
}
