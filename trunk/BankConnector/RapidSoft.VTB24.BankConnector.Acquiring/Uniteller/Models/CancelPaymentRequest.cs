namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models
{
    internal class CancelPaymentRequest
    {
        public enum ResponseFormat
        {
            Csv = 1,
            Wddx = 2,
            Xml = 3,
            Soap = 4
        }

        #region Обязательные

        public string ShopId { get; set; }

        public string BillNumber { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        #endregion

        #region Необязательные

        public ResponseFormat Format { get; set; }

        #endregion
    }
}
