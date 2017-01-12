namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Inputs
{
    public class UnitellerPayParameters
    {
        #region обязательные

        public string ShopId { get; set; }

        public string OrderId { get; set; }

        public decimal Subtotal { get; set; }

        public string ReturnUrlSuccess { get; set; }

        public string ReturnUrlFail { get; set; }

        #endregion

        #region необязательные

        public int? Lifetime { get; set; }

        public string CustomerId { get; set; }

        public bool Preauth { get; set; }

        #endregion
    }
}
