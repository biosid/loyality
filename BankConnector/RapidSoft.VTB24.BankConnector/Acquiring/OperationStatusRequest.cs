namespace RapidSoft.VTB24.BankConnector.Acquiring
{
    using System.Collections.Generic;
    using System.Net.Http;
	using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    public class RegistrationStatusRequest : IUnitellerRequest
    {
        public RegistrationStatusRequest(string orderId)
        {
			this.OrderId = orderId;
        }

		public string OrderId
		{
			get;
			set;
		}

        #region IUnitellerRequest Members

        public FormUrlEncodedContent GetFormContent()
        {
            return new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("Shop_ID", ConfigHelper.UnitellerRegisterShopId),
                    new KeyValuePair<string, string>("Login", ConfigHelper.UnitellerRegisterLogin),
                    new KeyValuePair<string, string>("Password", ConfigHelper.UnitellerPaymentPassword),
                    new KeyValuePair<string, string>("Format", ConfigHelper.UnitellerResultFormat.ToString()),
                    new KeyValuePair<string, string>("ShopOrderNumber", this.OrderId),
                    new KeyValuePair<string, string>("S_FIELDS", ConfigHelper.UnitellerResultFields),
                });
        }

        #endregion
    }
}