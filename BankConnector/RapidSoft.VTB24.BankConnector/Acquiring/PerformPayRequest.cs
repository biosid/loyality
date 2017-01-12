namespace RapidSoft.VTB24.BankConnector.Acquiring
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;

    public class PerformPayRequest : IUnitellerRequest
    {
        public PerformPayRequest(string context, string cid, bool doPay, int cv2)
        {
            this.Context = context;
            this.Cid = cid;
            this.DoPay = doPay ? "1" : "0";
            this.Cv2 = cv2;
        }

        public string Context
        {
            get;
            set;
        }

        public string Cid
        {
            get;
            set;
        }

        public string DoPay
        {
            get;
            set;
        }

        public int Cv2
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
                    new KeyValuePair<string, string>("context", this.Context), 
                    new KeyValuePair<string, string>("cid", this.Cid), 
                    new KeyValuePair<string, string>("doPay", this.DoPay), 
                    new KeyValuePair<string, string>("Cvc2", this.Cv2.ToString(CultureInfo.InvariantCulture))
                });
        }

        #endregion
    }
}