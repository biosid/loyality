namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models
{
    internal class GetAuthorizationResultsRequest
    {
        public enum ResponseFormat
        {
            Csv = 1,
            Wddx = 2,
            Parenthesized = 3,
            Xml = 4,
            Soap = 5
        }

        #region Обязательные

        public string ShopId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public ResponseFormat Format { get; set; }

        #endregion

        #region Необязательные

        public string ShopOrderNumber { get; set; }

        #endregion
    }
}
