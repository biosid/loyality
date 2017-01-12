namespace RapidSoft.VTB24.BankConnector.Acquiring
{
    using System.Collections.Generic;
    using System.Net.Http;

    public class ConfirmPayRequest : IUnitellerRequest
    {
        public ConfirmPayRequest(string context)
        {
            this.Context = context;
        }

        public string Context
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
                    new KeyValuePair<string, string>("context", this.Context)
                });
        }

        #endregion

        public string ToRequestString()
        {
            return string.Format("context={0}", this.Context);
        }
    }
}