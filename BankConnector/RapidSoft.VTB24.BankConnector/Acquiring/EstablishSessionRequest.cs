namespace RapidSoft.VTB24.BankConnector.Acquiring
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Net.Http;

    public class EstablishSessionRequest : IUnitellerRequest
    {
        private readonly UnitellerAuthInfo client;
        private readonly UnitellerSignatureCreator signature;

        public EstablishSessionRequest(UnitellerAuthInfo authInfo, UnitellerSignatureCreator signatureCreator)
        {
            Contract.Assert(authInfo != null);
            Contract.Assert(signatureCreator != null);

            client = authInfo;
            signature = signatureCreator;
        }

        #region IUnitellerRequest Members

        public FormUrlEncodedContent GetFormContent()
        {
            client.Validate();

            return new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("Shop_IDP", client.ShopId), 
                    new KeyValuePair<string, string>("Order_IDP", client.OrderId), 
                    new KeyValuePair<string, string>("Customer_IDP", client.CustomerId), 
                    new KeyValuePair<string, string>("Subtotal_P", client.SubtotalString), 
                    new KeyValuePair<string, string>("Signature", signature.GetSignatureHash(client)), 
                    new KeyValuePair<string, string>("URL_RETURN", client.ReturnUrl)
                });
        }

        #endregion
    }
}